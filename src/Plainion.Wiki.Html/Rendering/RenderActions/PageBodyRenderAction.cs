using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( PageBody ) )]
    public class PageBodyRenderAction : GenericRenderAction<PageBody>
    {
        /// <summary/>
        protected override void Render( PageBody body )
        {
            WriteLine( "<body>" );

            Render( body.Children );

            WriteLine( "</body>" );
        }
    }
}
