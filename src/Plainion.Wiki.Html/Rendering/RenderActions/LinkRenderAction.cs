using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( Link ) )]
    public class LinkRenderAction : GenericRenderAction<Link>
    {
        /// <summary/>
        protected override void Render( Link link )
        {
            Write( "<a href=\"" );
            Write( link.Url );
            Write( "\">" );
            Write( link.Text );
            Write( "</a>" );
        }
    }
}
