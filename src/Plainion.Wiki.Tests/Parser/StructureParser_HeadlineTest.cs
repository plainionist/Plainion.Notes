using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class StructureParser_HeadlineTest
    {
        private StructureParser myParser;

        [SetUp]
        public void SetUp()
        {
            myParser = new StructureParser();
        }

        [Test]
        public void SmallHeadline()
        {
            var headlineText = "hello world";
            var pageContent = "! " + headlineText;
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var headline = XAssert.HasSingleChildOf<Headline>( page );

            Assert.AreEqual( headlineText, headline.Text );
            Assert.AreEqual( 1, headline.Size );
        }

        [Test]
        public void BigHeadline()
        {
            var headlineText = "hello world";
            var pageContent = "!!! " + headlineText;
            var pageDesc = new FakePageDescriptor( pageContent );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var headline = XAssert.HasSingleChildOf<Headline>( page );

            Assert.AreEqual( headlineText, headline.Text );
            Assert.AreEqual( 3, headline.Size );
        }

        [Test]
        public void HeadlineWithParagraph()
        {
            var headlineText = "hello world";
            string[] para = 
                {
                    "first line",
                    "second line",
                    "third line"
                };
            var pageDesc = new FakePageDescriptor();
            pageDesc.AddContent( "!! " + headlineText );
            pageDesc.AddContent( para );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var headline = XAssert.HasSingleChildOf<Headline>( page );
            Assert.AreEqual( headlineText, headline.Text );
            Assert.AreEqual( 2, headline.Size );

            var paragraph = XAssert.HasSingleChildOf<Paragraph>( page );
            var textBlock = XAssert.HasSingleChildOf<TextBlock>( paragraph );
            Assert.AreEqual( string.Join( Environment.NewLine, para ), textBlock.Text() );
        }

        [Test]
        public void HeadlineWithEmptyLineAndParagraph()
        {
            var headlineText = "hello world";
            string[] para = 
                {
                    "first line",
                    "second line",
                    "third line"
                };
            var pageDesc = new FakePageDescriptor();
            pageDesc.AddContent( "!! " + headlineText );
            pageDesc.AddContent( string.Empty );
            pageDesc.AddContent( para );

            var page = myParser.Parse( pageDesc.Name, pageDesc.GetContent() );

            var headline = XAssert.HasSingleChildOf<Headline>( page );
            Assert.AreEqual( headlineText, headline.Text );
            Assert.AreEqual( 2, headline.Size );

            var paragraph = XAssert.HasSingleChildOf<Paragraph>( page );
            var textBlock = XAssert.HasSingleChildOf<TextBlock>( paragraph );
            Assert.AreEqual( string.Join( Environment.NewLine, para ), textBlock.Text() );
        }
    }
}
