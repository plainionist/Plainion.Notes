using System.Linq;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class ParagraphTest
    {
        [Test]
        public void CreateWithChildren()
        {
            var para = new Paragraph( new PlainText( "first" ) );

            Assert.AreEqual( 1, para.Children.Count() );

            var text = XAssert.HasSingleChildOf<TextBlock>( para );

            Assert.AreEqual( "first", text.Text() );
            Assert.AreEqual( text, para.Text );
        }

        [Test]
        public void ContainsEmptyTextBlockAfterCreation()
        {
            var para = new Paragraph();

            Assert.AreEqual( 1, para.Children.Count() );

            var text = XAssert.HasSingleChildOf<TextBlock>( para );

            Assert.IsNull( text.Text() );
            Assert.AreEqual( text, para.Text );
        }

        [Test]
        public void HandleNullAsEmptyChildrenOnCreation()
        {
            var para = new Paragraph( null );

            Assert.AreEqual( 1, para.Children.Count() );
        }

        [Test]
        public void CanConsume_TextBlockAndPreformattedText()
        {
            var para = new Paragraph();

            var bench = new TestBench();
            bench.ExpectConsumesOnly( para, new[] { typeof( TextBlock ), typeof( PreformattedText ) } );
        }

        [Test]
        public void ContainsOnlyOneTextBlock()
        {
            var para = new Paragraph();
            para.Consume( new TextBlock( "first" ) );
            para.Consume( new TextBlock( "second" ) );
            para.Consume( new TextBlock( "third" ) );

            Assert.AreEqual( 1, para.Children.Count() );

            var text = XAssert.HasSingleChildOf<TextBlock>( para );

            var lines = text.Text().AsLines().ToArray();

            Assert.AreEqual( 3, lines.Length );
            Assert.AreEqual( "first", lines[ 0 ] );
            Assert.AreEqual( "second", lines[ 1 ] );
            Assert.AreEqual( "third", lines[ 2 ] );
        }
    }
}
