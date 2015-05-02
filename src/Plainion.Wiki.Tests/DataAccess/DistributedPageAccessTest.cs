using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class DistributedPageAccessTest
    {
        [Test]
        public void Ctor_WithoutDefaultPageAccess_Throws()
        {
            Assert.Throws<ArgumentNullException>( () => new DistributedPageAccess( null ) );
        }

        [Test]
        public void Ctor_WithDefaultPageAccess_DefaultPageAccessPropertyShouldBeSet()
        {
            var pageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            var distributedPageAccess = new DistributedPageAccess( pageAccess );

            Assert.That( distributedPageAccess.DefaultPageAccess, Is.EqualTo( pageAccess ) );
        }

        [Test]
        public void Register_WithNullNamespace_Throws()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var pageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            Assert.Throws<ArgumentNullException>( () => distributedPageAccess.Register( null, pageAccess ) );
        }

        [Test]
        public void Register_WithNullPageAccess_Throws()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var anyNamespace = PageNamespace.Create( "e" );

            Assert.Throws<ArgumentNullException>( () => distributedPageAccess.Register( anyNamespace, null ) );
        }

        [Test]
        public void Register_WithEmptyNamespace_Throws()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var emptyNamespace = PageNamespace.Create();
            var pageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            Assert.Throws<ArgumentException>( () => distributedPageAccess.Register( emptyNamespace, pageAccess ) );
        }

        [Test]
        public void Register_NewNamespaceValidPageAccess_ShouldBeRegistered()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var anyNamespace = PageNamespace.Create( "e" );
            var anyPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            distributedPageAccess.Register( anyNamespace, anyPageAccess );

            var namespacePageAccessPair = distributedPageAccess.RegisteredPageAccesses.First();

            Assert.That( namespacePageAccessPair.Key, Is.EqualTo( anyNamespace ) );
            Assert.That( namespacePageAccessPair.Value, Is.EqualTo( anyPageAccess ) );
        }

        [Test]
        public void Register_AlreadyRegisteredNamespace_Throws()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var anyNamespace = PageNamespace.Create( "e" );
            var anyPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            distributedPageAccess.Register( anyNamespace, anyPageAccess );

            Assert.Throws<ArgumentException>( () => distributedPageAccess.Register( anyNamespace, anyPageAccess ) );
        }

        [Test]
        public void Unregister_UnknownNamespace_DoesNothing()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var anyNamespace = PageNamespace.Create( "e" );

            Assert.DoesNotThrow( () => distributedPageAccess.Unregister( anyNamespace ) );
        }

        [Test]
        public void Unregister_KnownNamespace_RemovesRegistration()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            var anyNamespace = PageNamespace.Create( "e" );
            var anyPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            distributedPageAccess.Register( anyNamespace, anyPageAccess );

            distributedPageAccess.Unregister( anyNamespace );

            Assert.That( distributedPageAccess.RegisteredPageAccesses, Is.Empty );
        }

        [Test]
        public void Pages_WithFilledDefaultPageAccess_ReturnsPagesFromDefaultPageAccess()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            Mock.Get( distributedPageAccess.DefaultPageAccess ).SetupGet( x => x.Pages ).Returns( Helpers.CreateUniqDummyPages() );

            Assert.That( distributedPageAccess.Pages, Is.EquivalentTo( distributedPageAccess.DefaultPageAccess.Pages ) );
        }

        [Test]
        public void Pages_RegisteredPageAccess_ReturnsPagesFromAllPageAccesses()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();
            Mock.Get( distributedPageAccess.DefaultPageAccess ).SetupGet( x => x.Pages ).Returns( Helpers.CreateUniqDummyPages() );

            var anyNamespace = PageNamespace.Create( "e" );
            var anyPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;
            Mock.Get( anyPageAccess ).SetupGet( x => x.Pages ).Returns( Helpers.CreateUniqDummyPages() );

            distributedPageAccess.Register( anyNamespace, anyPageAccess );

            var pageNamesFromDPA = GetNamesOfAllPagesWithoutNamespace( distributedPageAccess );
            var defaultPageAccessPageNames = GetNamesOfAllPagesWithoutNamespace( distributedPageAccess.DefaultPageAccess );
            var registeredPageAccessPageNames = GetNamesOfAllPagesWithoutNamespace( anyPageAccess );
            var allPageNames = defaultPageAccessPageNames.Concat( registeredPageAccessPageNames ).ToList();

            Assert.That( pageNamesFromDPA, Is.EquivalentTo( allPageNames ) );
        }

        [Test]
        public void Pages_RegisteredPageAccess_ReturnsPagesWithPrefixedNamespace()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();

            var anyNamespace = PageNamespace.Create( "e" );
            var anyPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;
            Mock.Get( anyPageAccess ).SetupGet( x => x.Pages ).Returns( Helpers.CreateUniqDummyPages() );

            distributedPageAccess.Register( anyNamespace, anyPageAccess );

            var pageNamesFromDPA = GetAllPageNames( distributedPageAccess );
            var prefixedPageNamesFromRegisteredPageAccess = anyPageAccess.Pages
                .Select( descriptor => PageName.Create( anyNamespace, descriptor.Name.Name ) )
                .ToList();

            Assert.That( pageNamesFromDPA, Is.EquivalentTo( prefixedPageNamesFromRegisteredPageAccess ) );
        }

        [Test]
        public void Pages_RegisteredPageAccessWithSubNamespaces_SubNamespacesArePreserved()
        {
            var distributedPageAccess = CreateDPAWithDefaultPageAccess();

            var anyNamespace = PageNamespace.Create( "e" );
            var anyPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;
            var pages = new[] { new InMemoryPageDescriptor( PageName.CreateFromPath( "/x/y" ) ) };
            Mock.Get( anyPageAccess ).SetupGet( x => x.Pages ).Returns( pages );

            distributedPageAccess.Register( anyNamespace, anyPageAccess );

            var pageNamesFromDPA = GetAllPageNames( distributedPageAccess );
            var expectedPageNames = new[] { PageName.CreateFromPath( "/e/x/y" ) };
            Assert.That( pageNamesFromDPA, Is.EquivalentTo( expectedPageNames ) );
        }

        private DistributedPageAccess CreateDPAWithDefaultPageAccess()
        {
            var pageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            return new DistributedPageAccess( pageAccess );
        }

        private IEnumerable<string> GetNamesOfAllPagesWithoutNamespace( IPageAccess pageAccess )
        {
            return pageAccess.Pages.Select( descriptor => descriptor.Name.Name ).ToList();
        }

        private IEnumerable<PageName> GetAllPageNames( IPageAccess pageAccess )
        {
            return pageAccess.Pages.Select( descriptor => descriptor.Name ).ToList();
        }
    }
}
