using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Generic RenderAction taking a lambda as real RenderAction.
    /// </summary>
    public class LambdaRenderAction<TNode> : IRenderAction
        where TNode : PageLeaf
    {
        private Action<TNode, IRenderActionContext> myRenderAction;

        /// <summary/>
        public LambdaRenderAction( Action<TNode, IRenderActionContext> renderAction )
        {
            if ( renderAction == null )
            {
                throw new ArgumentNullException( "renderAction" );
            }
            myRenderAction = renderAction;
        }

        /// <summary/>
        public void Render( PageLeaf node, IRenderActionContext context )
        {
            var typedNode = node as TNode;
            if ( typedNode == null )
            {
                throw new ArgumentException( "Given node expected to be of type: " + typeof( TNode ) );
            }

            myRenderAction( typedNode, context );
        }
    }
}
