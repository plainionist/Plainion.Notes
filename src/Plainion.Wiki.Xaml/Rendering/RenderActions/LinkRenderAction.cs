using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( Link ) )]
    public class LinkRenderAction : GenericRenderAction<Link>
    {
        protected override void Render( Link link )
        {
            Write( "<Hyperlink NavigateUri=\"" );
            Write( link.Url );
            Write( "\">" );
            Write( link.Text );
            Write( "</Hyperlink>" );
        }
    }
}
