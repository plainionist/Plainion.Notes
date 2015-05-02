using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Plainion.Httpd
{
    public static class HttpListenerRequestExtensions
    {
        public static NameValueCollection ParsePostContent( this HttpListenerRequest request )
        {
            if ( request.HttpMethod != "POST" )
            {
                return null;
            }

            using ( var reader = new StreamReader( request.InputStream, request.ContentEncoding ) )
            {
                return HttpUtility.ParseQueryString( reader.ReadToEnd(), request.ContentEncoding );
            }
        }
    }
}
