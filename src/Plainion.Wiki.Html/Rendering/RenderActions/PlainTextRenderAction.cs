using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( PlainText ) )]
    public class PlainTextRenderAction : GenericRenderAction<PlainText>
    {
        /// <summary/>
        protected override void Render( PlainText plainText )
        {
            WriteLine( plainText.Text );
        }
    }
}
