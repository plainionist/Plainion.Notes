using System;
using NUnit.Framework;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class AnchorTests : TestBase
    {
        [Test]
        public void Ctor_ValidName_NamePropertyShouldBeSet()
        {
            var anchor = new Anchor( "dummy" );

            Assert.AreEqual( "dummy", anchor.Name );
        }

        [Test]
        public void Ctor_InvalidName_ShouldThrowException( [Values( null, "", " " )] string name )
        {
            Assert.Throws<ArgumentNullException>( () => new Anchor( name ) );
        }

        [Test]
        public void Ctor_WhenCalled_ParentPropertyShouldBeNull()
        {
            var anchor = new Anchor( "dummy" );

            Assert.IsNull( anchor.Parent );
        }

        [Test]
        public override void Clone_WhenCalled_ShouldNotThrow()
        {
            Objects.Clone( new Anchor( "a" ) );
        }
    }
}
