using System.IO;
using System.Net;

namespace Plainion.Httpd.Views
{
    /// <summary>
    /// Designed to be the last view in the view stack of the 
    /// AdvancedHttpHandler. It will handle any request by generating a
    /// "cannot handle request" page.
    /// </summary>
    public class FallbackView : AbstractHttpView
    {
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return true;
        }

        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            var response = new HttpResponse();

            using ( var writer = new StreamWriter( response.OutputStream ) )
            {
                writer.WriteLine( "<html>" );
                writer.WriteLine( "<head><title>Request failed</title></head>" );
                writer.WriteLine( "<body>" );
                writer.WriteLine( "<h2>Unable to process request</h2>" );
                writer.WriteLine( request.RawUrl );
                writer.WriteLine( "</body>" );
                writer.WriteLine( "</html>" );
            }

            return response;
        }
    }
}
