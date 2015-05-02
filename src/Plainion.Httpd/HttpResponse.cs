using System.IO;
using System.Text;

namespace Plainion.Httpd
{
    public class HttpResponse
    {
        private MemoryStream myOutputStream;

        public HttpResponse()
            : this( null )
        {
        }

        public HttpResponse( string content )
        {
            Reset();

            if ( content != null )
            {
                using ( var writer = new StreamWriter( myOutputStream, Encoding.UTF8 ) )
                {
                    writer.WriteLine( content );
                }
            }
        }

        public string RedirectLocation
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public Stream OutputStream
        {
            get { return myOutputStream; }
        }

        internal byte[] GetContent()
        {
            return myOutputStream.ToArray();
        }

        internal void Reset()
        {
            ContentType = "text/html";
            RedirectLocation = null;
            myOutputStream = new MemoryStream();
        }
    }
}
