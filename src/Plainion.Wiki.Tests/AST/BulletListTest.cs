using Plainion.Wiki.AST;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class BulletListTest
    {
        [Test]
        public void OnlyConsumesListItems()
        {
            var list = new BulletList();

            var bench = new TestBench();
            bench.ExpectConsumesOnly( list, new[] { typeof( ListItem ), typeof( BulletList ) } );
        }

        [Test]
        public void ItemsProperty()
        {
            var items = new[] { new ListItem( "item1" ), new ListItem( "item2" ) };
            var list = new BulletList( items );

            CollectionAssert.AreEqual( items, list.Items );
        }
    }
}
