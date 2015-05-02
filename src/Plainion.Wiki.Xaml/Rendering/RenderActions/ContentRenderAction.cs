using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( Content ) )]
    public class ContentRenderAction : GenericRenderAction<Content>
    {
        protected override void Render( Content content )
        {
            Render( content.Children );
        }
    }
}
