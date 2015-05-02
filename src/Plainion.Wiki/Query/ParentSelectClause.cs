using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Selects the parent node (inside the AST) of the mached node.
    /// </summary>
    public class ParentSelectClause : AbstractMultiSelectClause
    {
        /// <summary/>
        protected override SelectedNodeHandle Select( PageLeaf node )
        {
            if ( node.Parent == null )
            {
                throw new InvalidOperationException( "Cannot select parent from node without parent" );
            }

            return new SelectedNodeHandle()
                {
                    SelectedNode = node.Parent,
                    QueryMatchCreator = () => new QueryMatch( node.Parent )
                };
        }
    }
}
