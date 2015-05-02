using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( PageBody ) )]
    public class PageBodyRenderAction : GenericRenderAction<PageBody>
    {
        protected override void Render( PageBody body )
        {
            WriteLine( "<Section>" );

            Render( body.Children );

            WriteLine( "</Section>" );
        }
    }
}
