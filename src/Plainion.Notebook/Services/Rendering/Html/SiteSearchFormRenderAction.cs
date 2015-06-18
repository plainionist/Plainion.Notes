using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Rendering;

namespace Plainion.Notebook.Services.Rendering.Html
{
    [HtmlRenderAction( typeof( SiteSearchForm ) )]
    public class SiteSearchFormRenderAction : GenericRenderAction<SiteSearchForm>
    {
        protected override void Render( SiteSearchForm from )
        {
            WriteLine( "<form method='get' action='/' style='display:inline;' target='_blank'>" );
            WriteLine( "    Full search: <input id='sitesearch' type='text' name='text' value='' />" );
            WriteLine( "    <input type='hidden' name='action' value='search'/>" );
            WriteLine( "</form>" );
        }
    }
}
