using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class InMemoryPageDescriptorTest
    {
        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithoutName()
        {
            new InMemoryPageDescriptor( null, "first line" );
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithNullContent()
        {
            new InMemoryPageDescriptor( PageName.Create( "SomePage" ), null );
        }

        [Test]
        public void CreateWithoutContent()
        {
            var pageName = PageName.Create( "SomePage" );
            var descriptor = new InMemoryPageDescriptor( pageName );

            Assert.AreEqual( pageName, descriptor.Name );
            Assert.AreEqual( 0, descriptor.GetContent().Length );
        }

        [Test]
        public void CreateWithContent()
        {
            var content = new[] { "line1", "line2" };
            var descriptor = new InMemoryPageDescriptor( PageName.Create( "SomePage" ), content );

            CollectionAssert.AreEqual( content, descriptor.GetContent() );
        }

        [Test]
        public void AddContent()
        {
            var descriptor = new InMemoryPageDescriptor( PageName.Create( "SomePage" ), "line1" );
            descriptor.AddContent( "line2" );

            CollectionAssert.AreEqual( new[] { "line1", "line2" }, descriptor.GetContent() );
        }
    }
}
