using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    [HtmlRenderAction( typeof( BulletList ) )]
    public class BulletListRenderAction : GenericRenderAction<BulletList>
    {
        protected override void Render( BulletList list )
        {
            string listType = list.Ordered ? "ol" : "ul";

            Write( "<" );
            Write( listType );
            WriteLine( ">" );

            Render( list.Children );

            Write( "</" );
            Write( listType );
            WriteLine( ">" );
        }
    }
}
