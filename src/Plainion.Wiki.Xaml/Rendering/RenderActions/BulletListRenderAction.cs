using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( BulletList ) )]
    public class BulletListRenderAction : GenericRenderAction<BulletList>
    {
        protected override void Render( BulletList list )
        {
            Write( "<List Margin=\"0\"" );
            if( list.Ordered )
            {
                Write( " StartIndex=\"1\"" );
            }
            WriteLine( ">" );

            Render( list.Children );

            Write( "</List>" );
        }
    }
}
