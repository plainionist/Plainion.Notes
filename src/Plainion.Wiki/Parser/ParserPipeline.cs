using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Parser.WikiText;

namespace Plainion.Wiki.Parser
{
    [Export( typeof( ParserPipeline ) )]
    public class ParserPipeline
    {
        public WikiWords WikiWords
        {
            get;
            set;
        }

        public PageBody Parse( IPageDescriptor page )
        {
            if ( WikiWords == null )
            {
                WikiWords = new WikiWords();
            }

            var parser = CreateParser( page );

            return parser.Parse();
        }

        private IPageContentParser CreateParser( IPageDescriptor page )
        {
            return new WikiTextParser( WikiWords, page.Name, page.GetContent() );
        }
    }
}
