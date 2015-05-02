using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( SiteSearchForm ) )]
    public class SiteSearchFormRenderAction : GenericRenderAction<SiteSearchForm>
    {
        /// <summary/>
        protected override void Render( SiteSearchForm from )
        {
            WriteLine( "<form method='get' action='/' style='display:inline;'>" );
            WriteLine( "    Full search: <input id='sitesearch' type='text' name='text' value='' />" );
            WriteLine( "    <input type='hidden' name='action' value='search'/>" );
            WriteLine( "</form>" );
        }
    }
}
