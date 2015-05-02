using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Parser.WikiText
{
    /// <summary/>
    public class WikiTextParser : IPageContentParser
    {
        private WikiWords myWikiWords;
        private PageName myPageName;
        private string[] myContent;

        /// <summary/>
        public WikiTextParser( WikiWords wikiWords, PageName pageName, string[] content )
        {
            myWikiWords = wikiWords;
            myPageName = pageName;
            myContent = content;
        }

        /// <summary/>
        public PageBody Parse()
        {
            var parser = new StructureParser();
            var pageBody = parser.Parse( myPageName, myContent );

            var markupParser = new MarkupParser( myWikiWords );
            markupParser.Parse( pageBody );

            return pageBody;
        }
    }
}
