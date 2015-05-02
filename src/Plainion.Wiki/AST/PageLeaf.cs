using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Childless base class of all AST elements.
    /// </summary>
    [Serializable]
    public abstract class PageLeaf
    {
        /// <summary/>
        protected PageLeaf()
        {
        }

        /// <summary/>
        public PageNode Parent
        {
            get;
            internal set;
        }

        /// <summary/>
        public PageNode GetRoot()
        {
            return GetRoot( this ) as PageNode;
        }

        private PageLeaf GetRoot( PageLeaf leaf )
        {
            return leaf.Parent != null ? GetRoot( leaf.Parent ) : leaf;
        }

        /// <summary/>
        public T GetParentOfType<T>() where T : PageLeaf
        {
            return GetParentOfType<T>( this );
        }

        private T GetParentOfType<T>( PageLeaf leaf ) where T : PageLeaf
        {
            if ( leaf is T )
            {
                return (T)leaf;
            }

            if ( leaf.Parent == null )
            {
                return null;
            }

            return GetParentOfType<T>( leaf.Parent );
        }
    }
}
