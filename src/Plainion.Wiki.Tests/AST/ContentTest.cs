using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class ContentTest
    {
        [Test]
        public void CreateEmpty()
        {
            var content = new Content();

            Assert.AreEqual( 0, content.Children.Count() );
        }

        [Test]
        public void CreateWithChildren()
        {
            var content = new Content( new PlainText( "hi" ) );

            Assert.AreEqual( 1, content.Children.Count() );

            var text = XAssert.HasSingleChildOf<PlainText>( content );

            Assert.AreEqual( "hi", text.Text );
        }

        [Test]
        public void ConsumeEverything()
        {
            var content = new Content();
            var body = new PageBody( content );

            content.Consume( new PlainText( "some text" ) );

            Assert.AreEqual( 1, content.Children.Count() );
            Assert.AreEqual( 1, body.Children.Count() );
        }
    }
}
