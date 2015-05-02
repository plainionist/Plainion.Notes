using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    [HtmlRenderAction( typeof( Paragraph ) )]
    public class ParagraphRenderAction : GenericRenderAction<Paragraph>
    {
        protected override void Render( Paragraph para )
        {
            WriteLine( "<p>" );

            Render( para.Children );

            WriteLine( "</p>" );
        }
    }
}
