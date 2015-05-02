using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( ListItem ) )]
    public class ListItemRenderAction : GenericRenderAction<ListItem>
    {
        /// <summary/>
        protected override void Render( ListItem listItem )
        {
            WriteLine( "<li>" );

            Render( listItem.Children );

            WriteLine( "</li>" );
        }
    }
}
