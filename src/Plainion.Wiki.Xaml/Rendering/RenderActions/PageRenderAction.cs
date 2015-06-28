using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( Page ) )]
    public class PageRenderAction : GenericRenderAction<Page>
    {
        protected override void Render( Page page )
        {
            WriteLine( "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" FontSize=\"13\">" );

            if( Context.RenderingContext.EngineContext.Config.RenderPageNameAsHeadline )
            {
                Write( "<Paragraph Name=\"pagetitle\" FontSize=\"15\">" );
                Write( page.Name.Name );
                WriteLine( "</Paragraph>" );
            }

            if( page.Content != null )
            {
                Render( page.Content );
            }

            WriteLine( "</FlowDocument>" );
        }
    }
}
