using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using List = Plainion.Wiki.AST.BulletList;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageBodyTest
    {
        [Test]
        public void CreateEmpty()
        {
            var body = new PageBody();

            Assert.IsNull( body.Name );
            Assert.AreEqual( 0, body.Children.Count ());
            Assert.AreEqual( PageBodyType.Content, body.Type );
        }

        [Test]
        public void CreateWithName()
        {
            var pageName = PageName.Create( "SomePage" );
            var body = new PageBody( pageName );

            Assert.AreEqual( pageName, body.Name );
        }

        [Test]
        public void CreateWithList()
        {
            var body = new PageBody( new List() );

            Assert.AreEqual( 1, body.Children.Count() );
            XAssert.HasSingleChildOf<List>( body );
        }

        [Test]
        public void ConsumeTextBlock()
        {
            var body = new PageBody();

            body.Consume( new TextBlock( "some text" ) );

            Assert.AreEqual( 1, body.Children.Count() );
            var para = XAssert.HasSingleChildOf<Paragraph>( body );
            var text = XAssert.HasSingleChildOf<TextBlock>( para );
            Assert.AreEqual( "some text", text.Text() );

            body.Consume( new TextBlock( "more text" ) );

            Assert.AreEqual( 2, body.Children.Count() );
            para = XAssert.NthChildIsOf<Paragraph>( body, 1 );
            text = XAssert.HasSingleChildOf<TextBlock>( para );
            Assert.AreEqual( "more text", text.Text() );
        }

        [Test]
        public void ConsumePlainText()
        {
            var body = new PageBody();

            body.Consume( new PlainText( "some text" ) );

            Assert.AreEqual( 1, body.Children.Count() );
            var para = XAssert.HasSingleChildOf<Paragraph>( body );
            var text = XAssert.HasSingleChildOf<TextBlock>( para );
            Assert.AreEqual( "some text", text.Text() );

            body.Consume( new PlainText( "more text" ) );

            Assert.AreEqual( 2, body.Children.Count() );
            para = XAssert.NthChildIsOf<Paragraph>( body, 1 );
            text = XAssert.HasSingleChildOf<TextBlock>( para );
            Assert.AreEqual( "more text", text.Text() );
        }
    }
}
