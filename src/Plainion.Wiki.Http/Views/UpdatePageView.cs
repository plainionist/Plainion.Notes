using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Plainion.Httpd.Views;
using Plainion.Httpd;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Http.Views
{
    /// <summary>
    /// Handles the update action to finally update the page.
    /// </summary>
    public class UpdatePageView : AbstractRenderView
    {
        /// <summary/>
        public UpdatePageView( IViewContext context )
            : base( context )
        {
        }

        /// <summary/>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return GetAction( request ) == "update";
        }

        /// <summary/>
        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            base.HandleRequest( request );

            var pageName = GetPageName( request );

            var content = GetPageContentFromPostRequest( request ).ToArray();
            if ( content.Length == 1 && content[ 0 ].Equals( "delete", StringComparison.OrdinalIgnoreCase ) )
            {
                Context.Engine.Delete( pageName );
            }
            else
            {
                Context.Engine.Update( pageName, content );
            }

            var response = new HttpResponse();
            response.RedirectLocation = GetPostEditRedirectLocation( pageName );

            return response;
        }
    }
}
