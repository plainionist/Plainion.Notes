using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Rendering
{
    [Export( typeof( RenderingPipeline ) )]
    public class RenderingPipeline
    {
        [ImportingConstructor]
        public RenderingPipeline( RenderingStepCatalog catalog, IRenderer renderer )
        {
            if ( catalog == null )
            {
                throw new ArgumentNullException( "catalog" );
            }
            if ( renderer == null )
            {
                throw new ArgumentNullException( "renderer" );
            }

            Renderer = renderer;

            Steps = catalog.Plugins
                .OrderBy( pair => pair.Key )
                .Select( pair => pair.Value )
                .ToList();
        }

        public IRenderer Renderer
        {
            get;
            private set;
        }

        public IEnumerable<IRenderingStep> Steps
        {
            get;
            private set;
        }

        public void Render( PageLeaf node, RenderingContext context )
        {
            foreach ( var step in Steps )
            {
                node = step.Transform( node, context.EngineContext );
            }

            Renderer.Render( node, context );
        }
    }
}
