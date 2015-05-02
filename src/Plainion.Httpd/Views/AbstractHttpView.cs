using System.Net;

namespace Plainion.Httpd.Views
{
    public abstract class AbstractHttpView
    {
        public abstract bool CanHandleRequest( HttpListenerRequest request );

        public abstract HttpResponse HandleRequest( HttpListenerRequest request );
    }
}
