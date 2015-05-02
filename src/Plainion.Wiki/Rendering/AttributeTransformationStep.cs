using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Resolves attributes which are used as "dynamic markup"/alias and transforms
    /// those into current AST representation.
    /// </summary>
    [RenderingStep( RenderingStage.AttributeTransformation )]
    public class AttributeTransformationStep : IRenderingStep
    {
        private PageAttributeTransformerCatalog myCatalog;

        /// <summary/>
        [ImportingConstructor]
        public AttributeTransformationStep( PageAttributeTransformerCatalog catalog )
        {
            if ( catalog == null )
            {
                throw new ArgumentNullException( "catalog" );
            }

            myCatalog = catalog;
        }

        /// <summary/>
        public PageLeaf Transform( PageLeaf node, EngineContext context )
        {
            if ( myCatalog.Plugins.Count == 0 )
            {
                return node;
            }

            var walker = new AstWalker<PageAttribute>( attr => Visit( attr, context ) );
            walker.Visit( node );

            return node;
        }

        private void Visit( PageAttribute attribute, EngineContext context )
        {
            if ( !myCatalog.Plugins.ContainsKey( attribute.FullName ) )
            {
                return;
            }

            var transformer = myCatalog.Plugins[ attribute.FullName ];
            transformer.Transform( attribute, context );
        }
    }
}
