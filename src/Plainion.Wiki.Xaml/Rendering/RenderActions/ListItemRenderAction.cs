using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using Plainion;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( ListItem ) )]
    public class ListItemRenderAction : GenericRenderAction<ListItem>
    {
        protected override void Render( ListItem listItem )
        {
            WriteLine( "<ListItem Padding=\"3\">" );

            var textBlock = listItem.Children.First() as TextBlock;
            Contract.Invariant( textBlock != null, "First child must be a TextBlock" );

            Render( new Paragraph( textBlock ) );

            if( listItem.Children.Count() > 1 )
            {
                Render( listItem.Children.Skip( 1 ) );
            }

            WriteLine( "</ListItem>" );
        }
    }
}
