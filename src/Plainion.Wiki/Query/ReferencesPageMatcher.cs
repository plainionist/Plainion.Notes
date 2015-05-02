using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using Plainion.Collections;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Searches for pages which reference the given page.
    /// </summary>
    public class ReferencesPageMatcher : IQueryMatcher
    {
        private PageName myPageName;

        /// <summary/>
        public ReferencesPageMatcher( PageName pageName )
        {
            if ( pageName == null )
            {
                throw new ArgumentNullException( "pageName" );
            }
            myPageName = pageName;
        }

        /// <summary/>
        public IEnumerable<QueryMatch> Match( PageHandle page )
        {
            var finder = new AstFinder<Link>( link => link.IsLinkingPage( myPageName ) );
            var result = finder.FirstOrDefault( page.Body );

            return QueryMatch.Bundle( result != null ? QueryMatch.CreatePageMatch( page.Name ) : null );
        }
    }
}
