using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( TextBlock ) )]
    public class TextBlockRenderAction : GenericRenderAction<TextBlock>
    {
        protected override void Render( TextBlock textBlock )
        {
            Render( textBlock.Children );
        }
    }
}
