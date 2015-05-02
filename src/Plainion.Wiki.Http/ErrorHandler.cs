using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Httpd.Views;
using System.Net;
using Plainion.Httpd;

namespace Plainion.Wiki.Http
{
    /// <summary/>
    public class ErrorHandler : GenericErrorHandler
    {
        private IEngine myEngine;

        /// <summary/>
        public ErrorHandler(IEngine engine)
        {
            myEngine = engine;
        }

        /// <summary/>
        public override HttpResponse GenerateResponse( Exception exception )
        {
            var response = new HttpResponse();

            var errorPage = myEngine.ErrorPageHandler.CreateGeneralErrorPage( exception.ToString() );
            myEngine.Render( errorPage, response.OutputStream );
            response.OutputStream.Close();

            return response;
        }
    }
}
