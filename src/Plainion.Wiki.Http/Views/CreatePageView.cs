using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Plainion.Httpd.Views;
using Plainion.Httpd;

namespace Plainion.Wiki.Http.Views
{
    /// <summary>
    /// Handles "create" action to finally create the page.
    /// </summary>
    public class CreatePageView : AbstractRenderView
    {
        /// <summary/>
        public CreatePageView( IViewContext context )
            : base( context )
        {
        }

        /// <summary/>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return GetAction( request ) == "create";
        }

        /// <summary/>
        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            base.HandleRequest( request );

            var pageName = GetPageName( request );

            Context.Engine.Create( pageName, GetPageContentFromPostRequest( request ) );

            var response = new HttpResponse();
            response.RedirectLocation = GetPostEditRedirectLocation( pageName );

            return response;
        }
    }
}
