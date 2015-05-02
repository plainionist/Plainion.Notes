using System.ComponentModel.Composition;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Rendering
{
    [Export( typeof( PageAttributeTransformerCatalog ) )]
    public class PageAttributeTransformerCatalog : AbstractPluginCatalog<string, IPageAttributeTransformer, IPageAttributeTransformerMetadata>
    {
        protected override string GetKey( IPageAttributeTransformerMetadata metadata )
        {
            return metadata.QualifiedAttributeName;
        }
    }
}
