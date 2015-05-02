using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Parser
{
    public class MarkupParser_TestBase
    {
        protected MarkupParser myParser;
        private PageBody myPage;
        private TextBlock myParserOutput;

        [SetUp]
        public void SetUp()
        {
            myParser = CreateParser();
            myPage = new PageBody();
        }

        protected virtual MarkupParser CreateParser()
        {
            return new MarkupParser();
        }

        [TearDown]
        public void TearDown()
        {
            myParserOutput = null;
            myPage = null;
            myParser = null;
        }

        protected void Assert_OutputEquals( params PageLeaf[] expected )
        {
            var actual = myParserOutput.Children;

            XAssert.ContentEquals( expected, actual.ToArray() );
        }

        protected void Parse( string text )
        {
            SetPageContent( text );
            myParserOutput = Parse();
        }

        private TextBlock Parse()
        {
            myParser.Parse( myPage );

            var para = XAssert.HasSingleChildOf<Paragraph>( myPage );
            var textBlock = XAssert.HasSingleChildOf<TextBlock>( para );

            return textBlock;
        }

        private void SetPageContent( string content )
        {
            var text = new PlainText();
            text.Consume( content );

            myPage.Consume( text );
        }
    }
}
