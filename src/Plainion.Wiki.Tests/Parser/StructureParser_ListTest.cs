using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using List = Plainion.Wiki.AST.BulletList;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class StructureParser_ListTest
    {
        private StructureParser myParser;

        [SetUp]
        public void SetUp()
        {
            myParser = new StructureParser();
        }

        [Test]
        public void FlatListWithMultilineItemsAndParagraph()
        {
            string[] pageContent = 
                {
                    " - item 1",
                    " - item 2",
                    "   is multiline",
                    string.Empty,
                    "new paragraph"
                };
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var list = XAssert.HasSingleChildOf<List>( page );
            var items = XAssert.HasNChildrenOf<ListItem>( list, 2 );

            Assert.AreEqual( "item 1", items[ 0 ].Text.Text() );
            Assert.AreEqual( "item 2" + Environment.NewLine + "is multiline", items[ 1 ].Text.Text() );

            var paragraph = XAssert.NthChildIsOf<Paragraph>( page, 1 );
            Assert.AreEqual( "new paragraph", paragraph.Text.Text() );
        }

        [Test]
        public void HeadlineWithList()
        {
            string[] pageContent = 
                {
                    "!! hi ho",
                    " - item 1",
                    " - item 2",
                };
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var headline = XAssert.NthChildIsOf<Headline>( page, 0 );
            Assert.AreEqual( "hi ho", headline.Text );

            var list = XAssert.NthChildIsOf<List>( page, 1 );
            var items = XAssert.HasNChildrenOf<ListItem>( list, 2 );

            Assert.AreEqual( "item 1", items[ 0 ].Text.Text() );
            Assert.AreEqual( "item 2", items[ 1 ].Text.Text() );

        }
    }
}
