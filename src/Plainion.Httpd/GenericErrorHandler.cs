using System;
using System.IO;

namespace Plainion.Httpd
{
    public class GenericErrorHandler
    {
        public virtual HttpResponse GenerateResponse( Exception exception )
        {
            var response = new HttpResponse();

            using ( var writer = new StreamWriter( response.OutputStream ) )
            {
                writer.WriteLine( "<html>" );
                writer.WriteLine( "<head><title>General Error</title></head>" );
                writer.WriteLine( "<body>" );
                writer.WriteLine( "<h2>General error occured</h2>" );
                writer.WriteLine( "<pre>" );
                writer.WriteLine( exception.ToString() );
                writer.WriteLine( "</pre>" );
                writer.WriteLine( "</body>" );
                writer.WriteLine( "</html>" );
            }

            return response;
        }
    }
}
