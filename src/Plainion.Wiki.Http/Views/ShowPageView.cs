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
    /// <summary/>
    public class ShowPageView : AbstractRenderView
    {
        /// <summary/>
        public ShowPageView( IViewContext context )
            : base( context )
        {
        }

        /// <summary/>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return GetAction( request ) == null;
        }

        /// <summary/>
        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            var pageName = GetPageName( request );

            var response = new HttpResponse();
            Context.Engine.Render( pageName, response.OutputStream );
            response.OutputStream.Close();

            return response;
        }
    }
}
