using Plainion.Wiki.AST;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class MarkupParser_LinkTest : MarkupParser_TestBase
    {
        [Test]
        public void SingleFreeExternalLink()
        {
            var url = "http://www.google.de";

            Parse( url );

            Assert_OutputEquals( new Link( url ) );
        }

        [Test]
        public void UrlWithPath()
        {
            var url = "http://www.bb-open.de/index.html";

            Parse( url );

            Assert_OutputEquals( new Link( url ) );
        }

        [Test]
        public void UrlWithPathAndQueryString()
        {
            var url = "http://www.bb-open.de/show?page=something";

            Parse( url );

            Assert_OutputEquals( new Link( url ) );
        }

        [Test]
        public void UrlWithEscapes()
        {
            var url = "http://www.ariva.de/xstrata_plc________dl-%2C50-aktie";

            Parse( url );

            Assert_OutputEquals( new Link( url ) );
        }

        [Test]
        public void FreeExternalLinkAtBeginning()
        {
            Parse( "see this http://www.google.de" );

            Assert_OutputEquals( new PlainText( "see this " ), new Link( "http://www.google.de" ) );
        }

        [Test]
        public void FreeExternalLinkAtEnd()
        {
            Parse( "http://www.google.de for more" );

            Assert_OutputEquals( new Link( "http://www.google.de" ), new PlainText( " for more" ) );
        }

        [Test]
        public void FreeExternalLinkÍnText()
        {
            Parse( "see this http://www.google.de for more" );

            Assert_OutputEquals( new PlainText( "see this " ),
                new Link( "http://www.google.de" ), new PlainText( " for more" ) );
        }

        [Test]
        public void SimpleWikiWord()
        {
            Parse( "see AnotherPage for more info" );

            Assert_OutputEquals( new PlainText( "see " ),
                new Link( "AnotherPage" ), new PlainText( " for more info" ) );
        }

        [Test]
        public void WikiWordWithQuestionMark()
        {
            Parse( "what about AnotherPage?" );

            Assert_OutputEquals( new PlainText( "what about " ),
                new Link( "AnotherPage" ), new PlainText( "?" ) );
        }

        [Test]
        public void SimpleTextWithSlashIsNoWikiWord()
        {
            Parse( "Sector: Erdöl/Erdgas" );

            Assert_OutputEquals( new PlainText( "Sector: Erdöl/Erdgas" ) );
        }

        [Test]
        public void NonWikiWordAbbreviation()
        {
            Parse( "see DEPage for more info" );

            Assert_OutputEquals( new PlainText( "see DEPage for more info" ) );
        }

        [Test]
        public void ExternalLinkInLinkMarkup()
        {
            Parse( "see [http://www.google.de] for more info" );

            Assert_OutputEquals( new PlainText( "see " ),
                new Link( "http://www.google.de" ), new PlainText( " for more info" ) );
        }

        [Test]
        public void ExternalLinkInLinkMarkupWithText()
        {
            Parse( "see [http://www.google.de|Google] for more info" );

            Assert_OutputEquals( new PlainText( "see " ),
                new Link( "http://www.google.de", "Google" ), new PlainText( " for more info" ) );
        }

        [Test]
        public void PageInLinkMarkup()
        {
            Parse( "see [DEPage] for more info" );

            Assert_OutputEquals( new PlainText( "see " ),
                new Link( "DEPage" ), new PlainText( " for more info" ) );
        }

        [Test]
        public void PageInLinkMarkupWithQuestionmark()
        {
            Parse( "what about [DEPage]?" );

            Assert_OutputEquals( new PlainText( "what about " ),
                new Link( "DEPage" ), new PlainText( "?" ) );
        }

        [Test]
        public void PageInLinkMarkupWithText()
        {
            Parse( "see [DEPage|German page] for more info" );

            Assert_OutputEquals( new PlainText( "see " ),
                new Link( "DEPage", "German page" ), new PlainText( " for more info" ) );
        }

        [Test]
        public void AnchorDefinition()
        {
            Parse( "see [#todo] for more info" );

            Assert_OutputEquals( new PlainText( "see " ),
                new Anchor( "todo" ), new PlainText( " for more info" ) );
        }
    }
}
