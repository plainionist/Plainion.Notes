using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace Plainion.Httpd.Views
{
    /// <summary>
    /// Handles requests for local files available in the given documentRoot.
    /// Supports different mime types and caching of the file content.
    /// </summary>
    public class StaticFileView : AbstractHttpView
    {
        private string myDocumentRoot;
        private Dictionary<string, byte[]> myFileCache;

        public StaticFileView( string documentRoot )
        {
            myDocumentRoot = documentRoot;

            myFileCache = new Dictionary<string, byte[]>();
        }

        /// <summary>
        /// Returns true if the requested file exists localy.
        /// </summary>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return File.Exists( GetLocalFile( request.Url ) );
        }

        private string GetLocalFile( Uri url )
        {
            var urlPath = HttpUtility.UrlDecode( url.AbsolutePath );
            if ( urlPath.StartsWith( "/" ) )
            {
                urlPath = urlPath.Substring( 1 );
            }

            var localPath = Path.Combine( myDocumentRoot, urlPath );

            localPath = localPath.Replace( '/', '\\' );
            localPath = Path.GetFullPath( localPath );

            return localPath;
        }

        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            var response = new HttpResponse();

            var localFile = GetLocalFile( request.Url );
            response.ContentType = GetMimeType( localFile );

            WriteFileContent( localFile, response );

            return response;
        }

        private void WriteFileContent( string file, HttpResponse response )
        {
            using ( var stream = new BufferedStream( new FileStream( file, FileMode.Open ) ) )
            {
                const int BlockSize = 4096;
                byte[] buffer = new byte[ BlockSize ];

                int readCount = 1;
                while ( ( readCount = stream.Read( buffer, 0, BlockSize ) ) > 0 )
                {
                    response.OutputStream.Write( buffer, 0, readCount );
                }
            }
        }

        private string GetMimeType( string file )
        {
            return MimeTypes.GetMimeTypeFromFile( file ) ?? "text/html";
        }
    }
}
