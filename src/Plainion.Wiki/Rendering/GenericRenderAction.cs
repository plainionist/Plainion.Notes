using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using System.IO;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    public abstract class GenericRenderAction<TNode, TContext> : IRenderAction
        where TNode : PageLeaf
        where TContext : IRenderActionContext
    {
        /// <summary/>
        protected TContext Context
        {
            get;
            private set;
        }

        /// <summary/>
        public void Render( PageLeaf node, IRenderActionContext context )
        {
            var typedNode = node as TNode;
            if ( typedNode == null )
            {
                throw new ArgumentException( "Given node expected to be of type: " + typeof( TNode ) );
            }

            Context = (TContext)context;

            Render( typedNode );

            // dont reset the context on leave because we might reuse 
            // RenderAction instances in recursion
            //Context = default( TContext );
        }

        /// <summary/>
        protected abstract void Render( TNode node );

        /// <summary/>
        protected void Render( IEnumerable<PageLeaf> nodes )
        {
            foreach ( var node in nodes )
            {
                Render( node );
            }
        }

        /// <summary/>
        protected void Render( PageLeaf node )
        {
            Context.Render( node );
        }

        /// <summary/>
        protected void Write( string text )
        {
            Context.RenderingContext.Writer.Write( text );
        }

        /// <summary/>
        protected void WriteLine( string text )
        {
            Context.RenderingContext.Writer.WriteLine( text );
        }
    }

    /// <summary/>
    public abstract class GenericRenderAction<TNode> : GenericRenderAction<TNode, IRenderActionContext>
        where TNode : PageLeaf
    {
    }
}
