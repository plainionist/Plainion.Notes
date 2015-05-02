using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    [HtmlRenderAction( typeof( HighlightText ) )]
    public class HighlightTextRenderAction : GenericRenderAction<HighlightText>
    {
        protected override void Render( HighlightText text )
        {
            Write( "<span style=\"background: " );

            if ( text.Level == 1 )
            {
                Write( "yellow" );
            }
            else if ( text.Level == 2 )
            {
                Write( "red" );
            }
            else
            {
                //nothing
                Write( "white" );
            }

            Write( "\">" );
            Write( text.Content );
            Write( "</span>" );
        }
    }
}
