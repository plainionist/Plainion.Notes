using System;
using System.Net;

namespace Plainion.Httpd
{
    public class HttpRequestEventArgs : EventArgs
    {
        public HttpRequestEventArgs( HttpListenerContext requestContext )
        {
            RequestContext = requestContext;
        }

        public HttpListenerContext RequestContext { get; private set; }
    }
}
