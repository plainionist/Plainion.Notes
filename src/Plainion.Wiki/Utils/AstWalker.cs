using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Utils
{
    /// <summary>
    /// Applies the given action to all nodes in the AST of the specified type.
    /// Algorithm: root first - children second
    /// </summary>
    public class AstWalker<T> where T : PageLeaf
    {
        private Action<T> myVisitAction;

        /// <summary/>
        public AstWalker( Action<T> visitAction )
        {
            if ( visitAction == null )
            {
                throw new ArgumentNullException( "visitAction" );
            }

            myVisitAction = visitAction;
        }

        /// <summary/>
        public void Visit( PageLeaf root )
        {
            if ( root is PageNode )
            {
                Visit( (PageNode)root );
            }
            else
            {
                CallVisitAction( root );
            }
        }

        private void Visit( PageNode root )
        {
            CallVisitAction( root );

            foreach ( var node in root.Children.ToList() )
            {
                Visit( node );
            }
        }

        private void CallVisitAction( PageLeaf node )
        {
            if ( node is T )
            {
                myVisitAction( (T)node );
            }
        }
    }
}
