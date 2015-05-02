using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( Headline ) )]
    public class HeadlineRenderAction : GenericRenderAction<Headline>
    {
        protected override void Render( Headline headline )
        {
            Write( "<Paragraph Name=\"" );
            Write( headline.Anchor );
            Write( "\"" );
            Write( " FontSize=\"" );
            Write( ( 18 - headline.Size ).ToString() );
            Write( "\">" );

            Write( "<Bold>" );
            Write( headline.Text );
            Write( "</Bold>" );

            WriteLine( "</Paragraph>" );
        }
    }
}
