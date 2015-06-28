using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( Paragraph ) )]
    public class ParagraphRenderAction : GenericRenderAction<Paragraph>
    {
        protected override void Render( Paragraph para )
        {
            WriteLine( "<Paragraph>" );

            Render( para.Children );

            WriteLine( "</Paragraph>" );
        }
    }
}
