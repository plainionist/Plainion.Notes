using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class AliasPageDescriptorTest 
    {
        [Test]
        public void Ctor_WithNullPageName_Throws()
        {
            var anyPageDescriptor = new Mock<IPageDescriptor> { DefaultValue = DefaultValue.Mock }.Object;

            Assert.Throws<ArgumentNullException>( () => new AliasPageDescriptor( null, anyPageDescriptor ) );
        }

        [Test]
        public void Ctor_WithNullPageDescriptor_Throws()
        {
            var anyPageName = PageName.Create( "a" );

            Assert.Throws<ArgumentNullException>( () => new AliasPageDescriptor( anyPageName, null ) );
        }

        [Test]
        public void Ctor_WithValidInput_PageNameAndPageDescriptorPropertiesShouldBeSet()
        {
            var anyPageName = PageName.Create( "a" );
            var anyPageDescriptor = new Mock<IPageDescriptor> { DefaultValue = DefaultValue.Mock }.Object;

            var aliasDescriptor = new AliasPageDescriptor( anyPageName, anyPageDescriptor );

            Assert.That( aliasDescriptor.Name, Is.EqualTo( anyPageName ) );
            Assert.That( aliasDescriptor.OriginalPageDescriptor, Is.EqualTo( anyPageDescriptor ) );
        }

        [Test]
        public void GetContent_WhenCalled_ReturnsContentOfOriginalPageDescriptor()
        {
            var anyPageName = PageName.Create( "a" );
            var anyPageDescriptor = new InMemoryPageDescriptor( PageName.Create( "b" ), "line1", "line2" );

            var aliasDescriptor = new AliasPageDescriptor( anyPageName, anyPageDescriptor );

            var content = aliasDescriptor.GetContent();

            Assert.That( content[ 0 ], Is.EqualTo( "line1" ) );
            Assert.That( content[ 1 ], Is.EqualTo( "line2" ) );
        }
    }
}
