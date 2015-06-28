using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( Anchor ) )]
    public class AnchorRenderAction : GenericRenderAction<Anchor>
    {
        protected override void Render( Anchor anchor )
        {
            Write( "<Span name=\"" );
            Write( anchor.Name );
            Write( "\"></Span>" );
        }
    }
}
