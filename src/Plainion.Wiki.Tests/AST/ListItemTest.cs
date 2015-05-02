using System.Linq;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using List = Plainion.Wiki.AST.BulletList;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class ListItemTest
    {
        [Test]
        public void CreateEmpty()
        {
            var item = new ListItem();

            Assert.IsNull( item.Text );
            Assert.AreEqual( 0, item.Children.Count() );
        }

        [Test]
        public void CreateWithText()
        {
            var item = new ListItem( "some text" );

            Assert.IsNotNull( item.Text );
            Assert.AreEqual( "some text", item.Text.Text() );
            Assert.AreEqual( 1, item.Children.Count() );
        }

        [Test]
        public void ContainsOnlyOneTextBlock()
        {
            var item = new ListItem( "some text" );
            item.Consume( new TextBlock( "more text" ) );
            item.Consume( new TextBlock( "much more" ) );

            Assert.AreEqual( 1, item.Children.Count() );

            var lines = item.Text.Text().AsLines().ToArray();

            Assert.AreEqual( 3, lines.Length );
            Assert.AreEqual( "some text", lines[ 0 ] );
            Assert.AreEqual( "more text", lines[ 1 ] );
            Assert.AreEqual( "much more", lines[ 2 ] );
        }

        [Test]
        public void WithTextAndList()
        {
            var item = new ListItem( "some text" );
            item.Consume( new List() );

            Assert.AreEqual( 2, item.Children.Count() );
            XAssert.HasSingleChildOf<TextBlock>( item );
            XAssert.HasSingleChildOf<List>( item );
        }

        [Test]
        public void OnlyConsumesTextAndLists()
        {
            var item = new ListItem();

            var bench = new TestBench();
            bench.ExpectConsumesOnly( item, new[] { typeof( TextBlock ), typeof( BulletList ) } );
        }
    }
}
