using System.Linq;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class TextBlockTest
    {
        [Test]
        public void CreateEmpty()
        {
            var text = new TextBlock();

            Assert.IsNull( text.Text() );
            Assert.AreEqual( 0, text.Children.Count() );
        }

        [Test]
        public void CreateWithText()
        {
            var text = new TextBlock( "hiho" );

            var plainText = XAssert.HasSingleChildOf<PlainText>( text );
            Assert.AreEqual( text, plainText.Parent );

            Assert.AreEqual( "hiho", text.Text() );
        }

        [Test]
        public void TextPropertyIsNullWithoutText()
        {
            var text = new TextBlock( new Anchor( "stuff" ) );

            XAssert.HasSingleChildOf<Anchor>( text );

            Assert.IsNull( text.Text() );
        }

        [Test]
        public void TextPropertyIsValidWithSingleChildOfTypeText()
        {
            var text = new TextBlock();
            text.Consume( new PlainText( "hiho" ) );

            Assert.AreEqual( 1, text.Children.Count() );
            Assert.AreEqual( "hiho", text.Text() );
        }

        [Test]
        public void TextPropertyIsNullWithMultipleChildren()
        {
            var text = new TextBlock();
            text.Consume( new Anchor( "stuff" ) );
            text.Consume( new PlainText( "second" ) );

            Assert.AreEqual( 2, text.Children.Count() );
            Assert.IsNull( text.Text() );
        }

        [Test]
        public void TextPropertyIsNullWithMultipleText()
        {
            var text = new TextBlock();
            text.Consume( new PlainText( "first" ) );
            text.Consume( new Anchor( "stuff" ) );
            text.Consume( new PlainText( "second" ) );

            Assert.AreEqual( 3, text.Children.Count() );
            Assert.IsNull( text.Text() );
        }

        [Test]
        public void OnlyConsumesTextAndMarkup()
        {
            var text = new TextBlock();

            var bench = new TestBench();
            bench.ExpectConsumesOnly( text, typeof( TextBlock ), typeof( PlainText ), typeof( Markup ) );
        }

        [Test]
        public void TextBlocksAreMergedWithNewLine()
        {
            var text = new TextBlock();
            text.Consume( new TextBlock( "first" ) );
            text.Consume( new TextBlock( "second" ) );

            XAssert.HasSingleChildOf<PlainText>( text );

            var lines = text.Text().AsLines().ToArray();

            Assert.AreEqual( 2, lines.Length );
            Assert.AreEqual( "first", lines[ 0 ] );
            Assert.AreEqual( "second", lines[ 1 ] );
        }

        [Test]
        public void MergePlainTextWithoutSeparater()
        {
            var text = new TextBlock();
            text.Consume( new PlainText( "first" ) );
            text.Consume( new PlainText( "second" ) );

            XAssert.HasSingleChildOf<PlainText>( text );

            Assert.AreEqual( "firstsecond", text.Text() );
        }

        [Test]
        public void DontMergePlainTextAndMarkups()
        {
            var text = new TextBlock();
            text.Consume( new PlainText( "first" ) );
            text.Consume( new Anchor( "stuff" ) );
            text.Consume( new PlainText( "second" ) );

            XAssert.HasNChildrenOf<PlainText>( text, 2 );
        }

        [Test]
        public void Consume_FirstPreformattedText_AddedAsChild()
        {
            var text = new TextBlock();
            text.Consume( new PreformattedText( "first" ) );

            XAssert.HasSingleChildOf<PreformattedText>( text );
        }

        [Test]
        public void Consume_SecondPreformattedText_MergedWithFirst()
        {
            var text = new TextBlock();
            text.Consume( new PreformattedText( "first" ) );
            text.Consume( new PreformattedText( "second" ) );

            var child = XAssert.HasSingleChildOf<PreformattedText>( text );
            Assert.That( child.Text, Is.EqualTo( "firstsecond" ) );
        }
    }
}
