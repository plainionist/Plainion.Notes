using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Plainion.Httpd.Views;
using Plainion.Httpd;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.AST;

namespace Plainion.Wiki.Http.Views
{
    /// <summary>
    /// Creates the page with edit form to create a new page.
    /// </summary>
    public class NewPageView : AbstractRenderView
    {
        private string myToolbar;

        /// <summary/>
        public NewPageView( IViewContext context, string toolbar )
            : base( context )
        {
            myToolbar = toolbar;
        }

        /// <summary/>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            var action = GetAction( request );
            return action == "new" ||
                ( action == "edit" && Context.Engine.Find( GetPageName( request ) ) == null );
        }

        /// <summary/>
        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            var response = new HttpResponse();

            var pageName = GetPageName( request );
            var page = CreateNewPage( pageName );

            Context.Engine.Render( page, response.OutputStream );
            response.OutputStream.Close();

            return response;
        }

        private PageBody CreateNewPage( PageName pageName )
        {
            var header = new HtmlBlock();
            header.AppendLine( "<h2>Create \"" + pageName.Name + "\"</h2>" );
            header.AppendLine( myToolbar );

            var action = pageName.FullName + "?action=create";
            var textContent = "Define " + pageName.Name + " here.";
            var form = HtmlUtils.CreateEditForm( action, textContent );

            var page = new PageBody( pageName );
            page.Type = PageBodyType.Edit;
            page.Consume( header );
            page.Consume( form );

            return page;
        }
    }
}
