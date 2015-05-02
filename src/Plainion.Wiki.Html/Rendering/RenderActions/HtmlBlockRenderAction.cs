using Plainion.Wiki.Html.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( HtmlBlock ) )]
    public class HtmlBlockRenderAction : GenericRenderAction<HtmlBlock>
    {
        /// <summary/>
        protected override void Render( HtmlBlock html )
        {
            WriteLine( html.Html );
        }
    }
}
