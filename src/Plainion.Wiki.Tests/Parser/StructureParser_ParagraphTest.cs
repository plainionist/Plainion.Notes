using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class StructureParser_ParagraphTest
    {
        private StructureParser myParser;

        [SetUp]
        public void SetUp()
        {
            myParser = new StructureParser();
        }

        [Test]
        public void OneLineParagraph()
        {
            var pageContent = "one simple paragraph with one line";
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var paragraph = XAssert.HasSingleChildOf<Paragraph>( page );
            var textBlock = XAssert.HasSingleChildOf<TextBlock>( paragraph );
            var plainText = XAssert.HasSingleChildOf<PlainText>( textBlock );

            Assert.AreEqual( pageContent, plainText.Text );
        }

        [Test]
        public void MultiLineParagraph()
        {
            string[] pageContent = 
                {
                    "first line",
                    "second line",
                    "third line"
                };
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var paragraph = XAssert.HasSingleChildOf<Paragraph>( page );
            var textBlock = XAssert.HasSingleChildOf<TextBlock>( paragraph );
            var plainText = XAssert.HasSingleChildOf<PlainText>( textBlock );

            Assert.AreEqual( string.Join( Environment.NewLine, pageContent ), plainText.Text );
        }

        [Test]
        public void EmptyPage()
        {
            var pageContent = "";
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            Assert.IsFalse( page.Children.Any() );
        }

        [Test]
        public void MultiLineMultiParagraph()
        {
            string[] para1 = 
                {
                    "first line of first paragraph",
                    "second line of first paragraph"
                };
            string[] para2 = 
                {
                    "first line of second paragraph",
                    "second line of second paragraph"
                };
            var pageDesc = new FakePageDescriptor();
            pageDesc.AddContent( para1 );
            pageDesc.AddContent( string.Empty );
            pageDesc.AddContent( para2 );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var paragraphs = XAssert.HasNChildrenOf<Paragraph>( page, 2 );

            var textBlock = XAssert.HasSingleChildOf<TextBlock>( paragraphs[ 0 ] );
            Assert.AreEqual( string.Join( Environment.NewLine, para1 ), textBlock.Text() );

            textBlock = XAssert.HasSingleChildOf<TextBlock>( paragraphs[ 1 ] );
            Assert.AreEqual( string.Join( Environment.NewLine, para2 ), textBlock.Text() );
        }

        [Test]
        public void ListIsNotAParagraph()
        {
            string[] pageContent = 
                {
                    " - first item",
                    " - second item",
                    " - third item"
                };
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            XAssert.HasNChildrenOf<Paragraph>( page, 0 );
        }
    }
}
