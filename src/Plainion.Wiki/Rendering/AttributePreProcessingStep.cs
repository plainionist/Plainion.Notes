using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Applies default preprocessing to remaning attributes.
    /// <remarks>
    /// Supported actions: ResolveAttributeReference, ResolveAttributeDefinition
    /// To disable or enhance certain features just derive from this step and override the
    /// appropriate functionallity.
    /// </remarks>
    /// </summary>
    [RenderingStep( RenderingStage.AttributePreProcessing )]
    public class AttributePreProcessingStep : IRenderingStep
    {
        /// <summary/>
        protected EngineContext Context { get; private set; }

        /// <summary/>
        public PageLeaf Transform( PageLeaf node, EngineContext context )
        {
            try
            {
                Context = context;

                var walker = new AstWalker<PageAttribute>( Visit );
                walker.Visit( node );

                return node;
            }
            finally
            {
                Context = null;
            }
        }

        private void Visit( PageAttribute attribute )
        {
            if ( attribute.IsDefinition )
            {
                ResolveAttributeDefinition( attribute );
            }
            else if ( !attribute.IsDefinition )
            {
                ResolveAttributeReference( attribute );
            }
        }

        /// <summary>
        /// If "RenderOnDefinition" is configured for that attribute the value 
        /// of the attribute is resolved.
        /// <see cref="ResolveAttributeValue"/>
        /// </summary>
        protected virtual void ResolveAttributeDefinition( PageAttribute attribute )
        {
            if ( attribute.Parent == null )
            {
                throw new ArgumentException( "Cannot handle attributes without parent" );
            }

            var attrConfig = GetPageAttributeConfig( attribute );
            if ( attrConfig == null || !attrConfig.IsRenderValueOnDefinition )
            {
                return;
            }

            var attrValue = ResolveAttributeValue( attrConfig.RenderValueOnDefinitionPrefix, attribute );

            // actually we want to do an insert here
            // (dont remove the attribute definition - it might break queries)
            var content = new Content();
            attribute.Parent.ReplaceChild( attribute, content );

            // insert instead of replace
            content.Consume( attribute );
            content.Consume( attrValue );
        }

        /// <summary>
        /// Returns the configuration of the given attribute.
        /// </summary>
        protected virtual AttributeRenderingStyle GetPageAttributeConfig( PageAttribute attribute )
        {
            var config = Context.Config.GetComponentConfig<AttributeRenderingSettings>( "AttributeRenderingSettings" );

            return config.Attributes.FirstOrDefault( cfg => cfg.QualifiedName == attribute.FullName );
        }

        /// <summary>
        /// Checks local page for existing attribute definition. If found it replaces the 
        /// reference with the defined attribute value.
        /// <see cref="ResolveAttributeValue"/>
        /// </summary>
        protected virtual void ResolveAttributeReference( PageAttribute attribute )
        {
            var body = attribute.GetParentOfType<PageBody>();
            if ( body == null )
            {
                return;
            }

            var finder = new AstFinder<PageAttribute>( attr => attr.IsDefinition && attr.FullName == attribute.FullName );
            var result = finder.FirstOrDefault( body );
            if ( result == null )
            {
                return;
            }

            var attrValue = ResolveAttributeValue( null, result );
            if ( attrValue == null )
            {
                return;
            }

            attribute.Parent.ReplaceChild( attribute, attrValue );
        }

        /// <summary>
        /// Resolves the value of the given attribute. 
        /// Turns the value into a link if it is an external url or an existing page.
        /// The prefix will be added to attribute value
        /// </summary>
        protected virtual Content ResolveAttributeValue( string prefix, PageAttribute attribute )
        {
            var content = new Content();

            if ( !string.IsNullOrEmpty( prefix ) )
            {
                content.Consume( new PlainText( prefix ) );
            }

            if ( string.IsNullOrEmpty( attribute.Value ) )
            {
                return content;
            }

            var link = new Link( attribute.Value );
            if ( link.IsExternal || Context.PageExists( PageName.CreateFromPath( link.Url ) ) )
            {
                content.Consume( link );
            }
            else
            {
                content.Consume( new PlainText( attribute.Value ) );
            }

            return content;
        }
    }
}
