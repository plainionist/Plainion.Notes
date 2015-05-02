
using System;
namespace Plainion.Wiki.AST
{
    /// <summary>
    /// A container for page leaves.
    /// Renderers will not render the container itself but only its content.
    /// Usually it is only used internally to transport a set of leaves or nodes 
    /// to the renderer or inside the render actions.
    /// </summary>
    [Serializable]
    public class Content : PageNode
    {
        /// <summary/>
        public Content( params PageLeaf[] children )
            : base( children )
        {
        }

        /// <summary>
        /// Accepts everything.
        /// </summary>
        protected override bool CanConsume( PageLeaf part )
        {
            return true;
        }

        /// <summary/>
        protected override void ConsumeInternal( PageLeaf part )
        {
            AddChild( part );
        }
    }
}
