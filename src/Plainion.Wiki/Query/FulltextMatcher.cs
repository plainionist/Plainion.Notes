using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using Plainion.Collections;
using Plainion;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Searches inside the name and the content of a page for the give substring.
    /// Case is ignored.
    /// </summary>
    public class FulltextMatcher : IQueryMatcher
    {
        /// <summary/>
        public FulltextMatcher( string searchText )
        {
            SearchText = string.IsNullOrWhiteSpace( searchText ) ? null : searchText;
        }

        /// <summary/>
        public string SearchText
        {
            get;
            private set;
        }

        /// <summary/>
        public IEnumerable<QueryMatch> Match( PageHandle page )
        {
            return QueryMatch.Bundle( page.Descriptor.Matches( SearchText ) ? QueryMatch.CreatePageMatch( page.Name ) : null );
        }
    }
}
