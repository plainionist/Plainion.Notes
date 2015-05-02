using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Utils
{
    /// <summary>
    /// Provides different APIs to search for nodes in the AST.
    /// Algorithm: root first - children second
    /// </summary>
    public class AstFinder<T> where T : PageLeaf
    {
        private Func<T, bool> myPredicate;

        /// <summary/>
        public AstFinder( Func<T, bool> predicate )
        {
            if ( predicate == null )
            {
                throw new ArgumentNullException( "predicate" );
            }

            myPredicate = predicate;
        }

        /// <summary/>
        public T FirstOrDefault( PageLeaf root )
        {
            return Where( root ).FirstOrDefault();
        }

        /// <summary/>
        public IEnumerable<T> Where( PageLeaf root )
        {
            if ( CallPredicate( root ) )
            {
                yield return root as T;
            }

            if ( root is PageNode )
            {
                var node = (PageNode)root;
                foreach ( var child in node.Children.ToList() )
                {
                    foreach ( var result in Where( child ) )
                    {
                        yield return result;
                    }
                }
            }
        }

        private bool CallPredicate( PageLeaf node )
        {
            return node is T ? myPredicate( (T)node ) : false;
        }
    }
}
