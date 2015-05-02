using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class DistributedPageAccess_WithRegistrationsTest
    {
        private DistributedPageAccess myDistributedPageAccess;
        private IPageAccess myPageAccessA;
        private PageNamespace myPageNamespaceA;

        [SetUp]
        public void SetUp()
        {
            var defaultPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;
            myDistributedPageAccess = new DistributedPageAccess( defaultPageAccess );

            myPageNamespaceA = PageNamespace.Create( "a" );
            myPageAccessA = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;

            myDistributedPageAccess.Register( myPageNamespaceA, myPageAccessA );

            // by default let Find() return null
            Mock.Get( myDistributedPageAccess.DefaultPageAccess ).Setup( x => x.Find( It.IsAny<PageName>() ) ).Returns<IPageDescriptor>( null );
            Mock.Get( myPageAccessA ).Setup( x => x.Find( It.IsAny<PageName>() ) ).Returns<IPageDescriptor>( null );
        }

        [Test]
        public void Find_PageNameFromRegisteredPageAccess_Succeeds()
        {
            var samplePage = Helpers.CreateEmptyPage( "p" );

            Mock.Get( myPageAccessA ).Setup( x => x.Find( samplePage.Name ) ).Returns( samplePage );

            var pageNameFromPageAccessA = PageName.Create( myPageNamespaceA, "p" );

            var page = myDistributedPageAccess.Find( pageNameFromPageAccessA );

            Assert.AreEqual( pageNameFromPageAccessA, page.Name );
        }

        [Test]
        public void Find_PageNameWithNamespaceFromRegisteredPageAccess_Succeeds()
        {
            var samplePage = Helpers.CreateEmptyPage( "p/1" );

            Mock.Get( myPageAccessA ).Setup( x => x.Find( samplePage.Name ) ).Returns( samplePage );

            var pageNameFromPageAccessA = PageName.CreateFromPath( myPageNamespaceA.AsPath + "/p/1" );

            var page = myDistributedPageAccess.Find( pageNameFromPageAccessA );

            Assert.AreEqual( pageNameFromPageAccessA, page.Name );
        }

        [Test]
        public void Find_PageNameFromDefaultPageAccess_Succeeds()
        {
            var samplePage = Helpers.CreateEmptyPage( "p" );

            Mock.Get( myDistributedPageAccess.DefaultPageAccess ).Setup( x => x.Find( samplePage.Name ) ).Returns( samplePage );

            var page = myDistributedPageAccess.Find( samplePage.Name );

            Assert.AreEqual( samplePage.Name, page.Name );
        }

        [Test]
        public void Find_UnknownPageName_Fails()
        {
            var samplePage = Helpers.CreateEmptyPage( "p" );

            Mock.Get( myDistributedPageAccess.DefaultPageAccess ).Setup( x => x.Find( samplePage.Name ) ).Returns( samplePage );
            Mock.Get( myPageAccessA ).Setup( x => x.Find( samplePage.Name ) ).Returns( samplePage );

            var page = myDistributedPageAccess.Find( PageName.Create( myPageNamespaceA, "non-existent" ) );

            Assert.IsNull( page );
        }

        [Test]
        public void Create_PageNameFromRegisteredPageAccess_ForwardsToRegisteredPageAccess()
        {
            var originalPageName = PageName.Create( "p" );

            var page = new InMemoryPageDescriptor( PageName.Create( myPageNamespaceA, "p" ), string.Empty );

            myDistributedPageAccess.Create( page );

            Mock.Get( myPageAccessA ).Verify( x => x.Create( It.Is<IPageDescriptor>( descriptor => descriptor.Name == originalPageName ) ), Times.Once() );
        }

        [Test]
        public void Create_PageNameWithNonRegisteredNamespace_ForwardsToDefaultPageAccess()
        {
            var originalPageName = PageName.CreateFromPath( "/notRegistered/p" );

            var page = new InMemoryPageDescriptor( originalPageName, string.Empty );

            myDistributedPageAccess.Create( page );

            Mock.Get( myDistributedPageAccess.DefaultPageAccess ).Verify( x => x.Create( It.Is<IPageDescriptor>( descriptor => descriptor.Name == originalPageName ) ), Times.Once() );
        }

        [Test]
        public void Update_PageNameFromRegisteredPageAccess_ForwardsToRegisteredPageAccess()
        {
            var originalPageName = PageName.Create( "p" );

            var page = new InMemoryPageDescriptor( PageName.Create( myPageNamespaceA, "p" ), string.Empty );

            myDistributedPageAccess.Update( page );

            Mock.Get( myPageAccessA ).Verify( x => x.Update( It.Is<IPageDescriptor>( descriptor => descriptor.Name == originalPageName ) ), Times.Once() );
        }

        [Test]
        public void Update_PageNameWithNonRegisteredNamespace_ForwardsToDefaultPageAccess()
        {
            var originalPageName = PageName.CreateFromPath( "/notRegistered/p" );

            var page = new InMemoryPageDescriptor( originalPageName, string.Empty );

            myDistributedPageAccess.Update( page );

            Mock.Get( myDistributedPageAccess.DefaultPageAccess ).Verify( x => x.Update( It.Is<IPageDescriptor>( descriptor => descriptor.Name == originalPageName ) ), Times.Once() );
        }

        [Test]
        public void Delete_PageNameFromRegisteredPageAccess_ForwardsToRegisteredPageAccess()
        {
            var originalPageName = PageName.Create( "p" );

            var page = new InMemoryPageDescriptor( PageName.Create( myPageNamespaceA, "p" ), string.Empty );

            myDistributedPageAccess.Delete( page );

            Mock.Get( myPageAccessA ).Verify( x => x.Delete( It.Is<IPageDescriptor>( descriptor => descriptor.Name == originalPageName ) ), Times.Once() );
        }

        [Test]
        public void Delete_PageNameWithNonRegisteredNamespace_ForwardsToDefaultPageAccess()
        {
            var originalPageName = PageName.CreateFromPath( "/notRegistered/p" );

            var page = new InMemoryPageDescriptor( originalPageName, string.Empty );

            myDistributedPageAccess.Delete( page );

            Mock.Get( myDistributedPageAccess.DefaultPageAccess ).Verify( x => x.Delete( It.Is<IPageDescriptor>( descriptor => descriptor.Name == originalPageName ) ), Times.Once() );
        }
    }
}
