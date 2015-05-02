using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    [HtmlRenderAction( typeof( PreformattedText ) )]
    public class PreformattedTextRenderAction : GenericRenderAction<PreformattedText>
    {
        protected override void Render( PreformattedText text )
        {
            WriteLine( "<pre>" );
            WriteLine( text.Text );
            WriteLine( "</pre>" );
        }
    }
}
