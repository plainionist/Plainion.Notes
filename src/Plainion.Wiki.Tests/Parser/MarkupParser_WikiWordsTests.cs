using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class MarkupParser_WikiWordsTests : MarkupParser_TestBase
    {
        private WikiWords myWikiWords;

        protected override MarkupParser CreateParser()
        {
            myWikiWords = new WikiWords();
            return new MarkupParser( myWikiWords );
        }

        [Test]
        public void EmptyWikiWords()
        {
            Parse( "simple text" );

            Assert_OutputEquals( new PlainText( "simple text" ) );
        }

        [Test]
        public void SomeWikiWords()
        {
            LoadWikiWords( "two" );

            Parse( "one two three" );

            Assert_OutputEquals( new PlainText( "one " ), new Link( "two" ), new PlainText( " three" ) );
        }

        private void LoadWikiWords( params string[] words )
        {
            myWikiWords.Add( words);
        }
    }
}
