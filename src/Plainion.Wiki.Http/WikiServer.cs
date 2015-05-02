using System;
using System.ComponentModel.Composition;
using Plainion.Httpd;

namespace Plainion.Wiki.Http
{
    [Export( typeof( WikiServer ) )]
    public class WikiServer
    {
        private HttpDaemon myWebServer;
        private IServerSite mySite;
        private object myLock;

        [ImportingConstructor]
        public WikiServer( IServerSite site, IEngine engine )
        {
            if ( site == null )
            {
                throw new ArgumentNullException( "site" );
            }
            mySite = site;

            myLock = new object();
            Controller = new BasicHttpController( engine, mySite.DocumentRoot );

            DocumentRootUrl = "http://localhost:" + mySite.Port;
            myWebServer = new HttpDaemon( new Uri( DocumentRootUrl ) );
        }

        public string DocumentRootUrl
        {
            get;
            private set;
        }

        public BasicHttpController Controller
        {
            get;
            private set;
        }

        public void Start()
        {
            myWebServer.IncomingRequest += OnIncomingRequest;
            myWebServer.Start();
        }

        private void OnIncomingRequest( object sender, HttpRequestEventArgs e )
        {
            lock ( myLock )
            {
                Console.WriteLine( "Request: " + e.RequestContext.Request.Url.PathAndQuery );

                Controller.HandleRequest( e.RequestContext );
            }
        }

        public void Stop()
        {
            myWebServer.IncomingRequest -= OnIncomingRequest;
            myWebServer.Stop();
        }
    }
}