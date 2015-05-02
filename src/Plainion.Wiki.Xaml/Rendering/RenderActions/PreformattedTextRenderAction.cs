using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( PreformattedText ) )]
    public class PreformattedTextRenderAction : GenericRenderAction<PreformattedText>
    {
        protected override void Render( PreformattedText text )
        {
            WriteLine( "<Paragraph FontFamiliy=\"Courier New\">" );
            WriteLine( text.Text );
            WriteLine( "</Paragraph>" );
        }
    }
}
