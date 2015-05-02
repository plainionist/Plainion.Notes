using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( HighlightText ) )]
    public class HighlightTextRenderAction : GenericRenderAction<HighlightText>
    {
        protected override void Render( HighlightText text )
        {
            Write( "<Span Background=\"" );

            if( text.Level == 1 )
            {
                Write( "Yellow" );
            }
            else if( text.Level == 2 )
            {
                Write( "Red" );
            }
            else
            {
                //nothing
                Write( "White" );
            }

            Write( "\">" );

            Write( text.Content );

            Write( "</Span>" );
        }
    }
}
