using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( Content ) )]
    public class ContentRenderAction : GenericRenderAction<Content>
    {
        /// <summary/>
        protected override void Render( Content content )
        {
            Render( content.Children );
        }
    }
}
