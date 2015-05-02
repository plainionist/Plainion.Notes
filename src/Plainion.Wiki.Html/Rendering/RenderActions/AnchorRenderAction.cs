using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( Anchor ) )]
    public class AnchorRenderAction : GenericRenderAction<Anchor>
    {
        /// <summary/>
        protected override void Render( Anchor anchor )
        {
            Write( "<a name=\"" );
            Write( anchor.Name );
            Write( "\"></a>" );
        }
    }
}
