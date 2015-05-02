using Plainion.Wiki.AST;
using Plainion;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Creates a deep copy of the AST because the subsequent steps
    /// will modify the AST dynamically but each rendering has to 
    /// start with the original AST.
    /// </summary>
    [RenderingStep( RenderingStage.Clone )]
    public class CloneStep : IRenderingStep
    {
        /// <summary/>
        public PageLeaf Transform( PageLeaf node, EngineContext context )
        {
            return Objects.Clone( node );
        }
    }
}
