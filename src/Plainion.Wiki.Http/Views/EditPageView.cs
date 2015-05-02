using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Plainion.Httpd.Views;
using Plainion.Httpd;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Html.AST;

namespace Plainion.Wiki.Http.Views
{
    /// <summary>
    /// Creates a page with edit form to update a page.
    /// </summary>
    public class EditPageView : AbstractRenderView
    {
        private string myToolbar;

        /// <summary/>
        public EditPageView( IViewContext context, string toolbar )
            : base( context )
        {
            myToolbar = toolbar;
        }

        /// <summary/>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return GetAction( request ) == "edit"
                && Context.Engine.Find( GetPageName( request ) ) != null;
        }

        /// <summary/>
        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            var response = new HttpResponse();

            var pageName = GetPageName( request );
            var pageDescriptor = Context.Engine.Find( pageName );

            var page = CreateResponsePage( pageDescriptor );

            Context.Engine.Render( page, response.OutputStream );
            response.OutputStream.Close();

            return response;
        }

        private PageBody CreateResponsePage( IPageDescriptor pageDescriptor )
        {
            var header = new HtmlBlock();
            header.AppendLine( "<h2 id='edit-header'>Edit \"" + pageDescriptor.Name + "\"</h2>" );
            header.AppendLine( myToolbar );

            var action = pageDescriptor.Name + "?action=update";
            var textContent = pageDescriptor.GetContent();
            var form = HtmlUtils.CreateEditForm( action, textContent );

            var page = new PageBody( pageDescriptor.Name );
            page.Type = PageBodyType.Edit;
            page.Consume( header );
            page.Consume( form );

            return page;
        }
    }
}
