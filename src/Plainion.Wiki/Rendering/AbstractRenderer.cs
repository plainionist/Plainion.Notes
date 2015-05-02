using System;
using System.Collections.Generic;
using Plainion.Wiki.AST;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Baseclass for renderers with plugable RenderActions.
    /// </summary>
    [InheritedExport( typeof( IRenderer ) )]
    public abstract class AbstractRenderer : IRenderer, IRenderActionContext
    {
        private RenderingContext myContext;

        /// <summary/>
        protected AbstractRenderer()
        {
        }

        /// <summary/>
        protected abstract IDictionary<Type, IRenderAction> RenderActions
        {
            get;
        }

        /// <summary/>
        public event EventHandler PreRendering;

        /// <summary/>
        public event EventHandler PostRendering;

        /// <summary>
        /// Rendering entry point.
        /// </summary>
        public void Render( PageLeaf root, RenderingContext context )
        {
            try
            {
                myContext = context;
                Root = root;

                FirePreRendering();

                Render( root );

                FirePostRendering();
            }
            finally
            {
                myContext = null;
                Root = null;
            }
        }

        private void FirePreRendering()
        {
            if ( PreRendering != null )
            {
                PreRendering( this, EventArgs.Empty );
            }
        }

        private void FirePostRendering()
        {
            if ( PostRendering != null )
            {
                PostRendering( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// API for RenderActions to trigger a rendering of AST children.
        /// </summary>
        public void Render( PageLeaf node )
        {
            if ( myContext == null )
            {
                throw new InvalidOperationException( "Render called outside rendering process" );
            }

            var type = node.GetType();
            while ( type != typeof( object ) )
            {
                if ( RenderActions.ContainsKey( type ) )
                {
                    break;
                }

                type = type.BaseType;
            }

            if ( RenderActions.ContainsKey( type ) )
            {
                var renderAction = RenderActions[ type ];
                renderAction.Render( node, this );
            }
        }

        /// <summary/>
        public RenderingContext RenderingContext
        {
            get { return myContext; }
        }

        /// <summary/>
        public PageLeaf Root
        {
            get;
            private set;
        }
    }
}
