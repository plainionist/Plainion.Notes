using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// This from clause will not allow pages which are neither children
    /// nor siblings.
    /// </summary>
    public class NoParentFromClause : IFromClause
    {
        private PageName myRoot;

        /// <summary/>
        public NoParentFromClause( PageName root )
        {
            if ( root == null )
            {
                throw new ArgumentNullException( "root" );
            }

            myRoot = root;
        }

        /// <summary/>
        public bool IsQueryFromPageAllowed( PageBody page )
        {
            return page.Name.Namespace.StartsWith( myRoot.Namespace );
        }
    }
}
