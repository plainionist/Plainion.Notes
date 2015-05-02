using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( Headline ) )]
    public class HeadlineRenderAction : GenericRenderAction<Headline>
    {
        /// <summary/>
        protected override void Render( Headline headline )
        {
            Write( "<a name=\"" );
            Write( headline.Anchor );
            Write( "\">" );

            string size = headline.Size.ToString();
            Write( "<h" );
            Write( size );
            Write( ">" );
            Write( headline.Text );
            Write( "</h" );
            Write( size );
            Write( ">" );

            WriteLine( "</a>" );
        }
    }
}
