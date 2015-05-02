using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using List = Plainion.Wiki.AST.BulletList;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class ListParserTest
    {
        private ListParser myParser;
        private PageBody myPageBody;

        [SetUp]
        public void SetUp()
        {
            myPageBody = new PageBody();
            myParser = new ListParser( myPageBody );
        }

        [Test]
        public void FlatListWithIndention()
        {
            myParser.Parse(
                     " - item 1",
                     " - item 2" );

            Assert_ListEquals( OuterList, "item 1", "item 2" );
        }

        [Test]
        public void FlatListWithoutIndention()
        {
            myParser.Parse(
                     "- item 1",
                     "- item 2" );

            Assert_ListEquals( OuterList, "item 1", "item 2" );
        }

        [Test]
        public void FlatListWithMultilineItems()
        {
            myParser.Parse(
                    " - item 1",
                    " - item 2",
                    "   is multiline",
                    " - item 3",
                    "   is also multiline" );

            Assert_ListEquals( OuterList,
                "item 1",
                "item 2" + Environment.NewLine + "is multiline",
                "item 3" + Environment.NewLine + "is also multiline" );
        }

        [Test]
        public void ListTree()
        {
            myParser.Parse(
                    "- item 1",
                    "   - item a",
                    "   - item b",
                    "- item 2" );

            Assert_ListEquals( OuterList, "item 1", "item 2" );

            var innerList = XAssert.HasSingleChildOf<List>( OuterList.Items.First() );

            Assert_ListEquals( innerList, "item a", "item b" );
        }

        [Test]
        public void ListTreeWithMultilineItems()
        {
            myParser.Parse(
                     "- item 1",
                     "   - item a",
                     "     and more",
                     "   - item b",
                     "     and even more",
                     "- item 2" );

            Assert_ListEquals( OuterList, "item 1", "item 2" );

            var innerList = XAssert.HasSingleChildOf<List>( OuterList.Items.First() );

            Assert_ListEquals( innerList,
                "item a" + Environment.NewLine + "and more",
                "item b" + Environment.NewLine + "and even more" );
        }

        private List OuterList
        {
            get { return myPageBody.Children.OfType<List>().Single(); }
        }

        private void Assert_ListEquals( List list, params string[] expected )
        {
            var actual = list.Items.Select( item => item.Text.Text() );

            CollectionAssert.AreEqual( expected, actual );
        }
    }
}
