using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Collections;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Base class of AST elements that support children.
    /// </summary>
    [Serializable]
    public abstract class PageNode : PageLeaf
    {
        private List<PageLeaf> myChildren;

        /// <summary/>
        protected PageNode( params PageLeaf[] children )
        {
            if ( children == null )
            {
                throw new ArgumentNullException( "children" );
            }

            myChildren = new List<PageLeaf>();

            foreach ( var child in children )
            {
                Consume( child );
            }
        }

        /// <summary/>
        public IEnumerable<PageLeaf> Children
        {
            get { return myChildren; }
        }

        /// <summary/>
        protected internal void RemoveAllChildren()
        {
            foreach ( var child in myChildren )
            {
                child.Parent = null;
            }
            myChildren.Clear();
        }

        /// <summary/>
        protected void AddChild( PageLeaf child )
        {
            child.Parent = this;
            myChildren.Add( child );
        }

        /// <summary/>
        public void RemoveChild( PageLeaf child )
        {
            if ( child == null )
            {
                throw new ArgumentNullException( "child" );
            }

            if ( myChildren.Contains( child ) )
            {
                child.Parent = null;
                myChildren.Remove( child );
            }
        }

        /// <summary>
        /// Just replaces the child by the new one.
        /// No check whether this node "can consume" it.
        /// </summary>
        public void ReplaceChild( PageLeaf source, PageLeaf target )
        {
            source.Parent = null;
            target.Parent = this;

            var childPos = myChildren.IndexOf( source );
            myChildren[ childPos ] = target;
        }

        /// <summary>
        /// Implements the "consume protocol".
        /// Asks implementation if the given part can be consumed. 
        /// If yes <see cref="ConsumeInternal(PageLeaf)"/> is called.
        /// If no Consume one the parent is called. If Parent is null an exception
        /// will be thrown.
        /// </summary>
        public void Consume( PageLeaf part )
        {
            if ( CanConsume( part ) )
            {
                ConsumeInternal( part );
                return;
            }

            if ( Parent == null )
            {
                throw new InvalidOperationException( "Cannot consume: " + part );
            }

            Parent.Consume( part );
        }

        /// <summary>
        /// Defines which kind of elements can be consumed.
        /// Default: nothing.
        /// </summary>
        protected virtual bool CanConsume( PageLeaf part )
        {
            return false;
        }

        /// <summary>
        /// Defines how the given part will be consumed (e.g. added to the Children
        /// property directly or modified beforehand).
        /// Default: nothing.
        /// </summary>
        protected virtual void ConsumeInternal( PageLeaf part )
        {
        }
    }
}
