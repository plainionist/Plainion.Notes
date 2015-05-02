using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Creates a QueryMatch for the whole page.
    /// </summary>
    public class PageSelectClause : ISelectClause
    {
        /// <summary>
        /// It needs to be quaranteed that all nodes belong to the same page.
        /// </summary>
        public IEnumerable<QueryMatch> Select( IEnumerable<PageLeaf> nodes )
        {
            return nodes.Select( node => CreateQueryMatch( node ) )
                .Take( 1 )
                .ToList();
        }

        private QueryMatch CreateQueryMatch( PageLeaf node )
        {
            var pageBody = node.GetParentOfType<PageBody>();
            if ( pageBody == null )
            {
                throw new InvalidOperationException( "Cannot create page match from a node without PageBody parent" );
            }

            return QueryMatch.CreatePageMatch( pageBody.Name );
        }
    }
}
