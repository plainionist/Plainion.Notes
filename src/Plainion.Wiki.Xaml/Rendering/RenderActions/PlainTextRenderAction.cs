using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( PlainText ) )]
    public class PlainTextRenderAction : GenericRenderAction<PlainText>
    {
        protected override void Render( PlainText plainText )
        {
            WriteLine( "<Run>" );
            WriteLine( plainText.Text );
            WriteLine( "</Run>" );
        }
    }
}
