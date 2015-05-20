using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Awesomium.Core;
using Plainion.IO;
using Plainion.IO.RealFS;
using Plainion.Notebook.Model;
using Plainion.Wiki;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.DataAccess.FlatFile;
using Plainion.Wiki.Html;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Http;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion;
using Plainion.Composition;
using System.Reflection;
using Plainion.Prism.Events;

namespace Plainion.Notebook.Services
{
    [Export]
    class WikiService : IDisposable
    {
        private WikiServer myDaemon;
        private Composer myComposer;
        private Uri myHomePageUri;

        [ImportingConstructor]
        public WikiService( IEventAggregator eventAggregator )
        {
            eventAggregator.GetEvent<ApplicationShutdownEvent>().Subscribe( x => OnShutdown() );
        }

        private void OnShutdown()
        {
            if( myDaemon != null )
            {
                myDaemon.Stop();
            }
        }

        public void StartDaemon( Project project )
        {
            if( !Directory.Exists( project.PagesRoot ) )
            {
                Directory.CreateDirectory( project.PagesRoot );
            }

            myComposer = new Composer();

            var fs = new FileSystemImpl();
            myComposer.RegisterInstance<IFileSystem>( fs );
            myComposer.RegisterInstance( CompositionContractNames.FileSystemRoot, fs.Directory( project.PagesRoot ) );

            myComposer.Register(
                typeof( DefaultFlatFileCompositionDescriptor ),
                typeof( DefaultHtmlCompsitionDescriptor ),
                typeof( DefaultHttpCompositionDescriptor ),
                typeof( FlatFilePageHistoryAccess ),
                typeof( HtmlRenderer ),
                typeof( HtmlRenderActionCatalog ),
                typeof( RenderingPipeline ),
                typeof( RenderingStepCatalog ),
                typeof( PageAttributeTransformerCatalog ),
                typeof( QueryCompiler ),
                typeof( DefaultAuditingLog ),
                typeof( Engine ),
                typeof( PageRepository ),
                typeof( ParserPipeline ),
                typeof( DefaultErrorPageHandler )
            );

            myComposer.Register( typeof( WikiServer ) );

            var decoratorCatalog = new DecoratorChainCatalog( typeof( IPageAccess ) );
            decoratorCatalog.Add( typeof( FlatFilePageAccess ) );
            decoratorCatalog.Add( typeof( AuditingPageAccessDecorator ) );
            myComposer.Add( decoratorCatalog );

            myComposer.RegisterRenderActions( typeof( HtmlRenderer ).Assembly );
            myComposer.RegisterRenderActions( GetType().Assembly );
            myComposer.RegisterRenderingSteps( typeof( Engine ).Assembly );
            myComposer.RegisterPageAttributeTransformers( typeof( Engine ).Assembly );

            var serverSite = new DefaultServerSite( project.PagesRoot, GetRandomUnusedPort() );
            myComposer.RegisterInstance<IServerSite>( serverSite );

            myComposer.Compose();

            var siteConfig = myComposer.Resolve<SiteConfig>( CompositionContractNames.SiteConfig );
            siteConfig.RenderPageNameAsHeadline = false;

            myDaemon = myComposer.Resolve<WikiServer>();
            myDaemon.Start();

            myHomePageUri = new Uri( myDaemon.DocumentRootUrl );

            Debug.WriteLine( string.Format( "Plainion.Wiki server listening to {0}", myDaemon.DocumentRootUrl ) );

            if( !WebCore.IsInitialized )
            {
                WebCore.Initialize( new WebConfig()
                {
                    LogPath = null,
                    LogLevel = LogLevel.None
                } );
            }
        }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener( IPAddress.Loopback, 0 );
            listener.Start();
            var port = ( ( IPEndPoint )listener.LocalEndpoint ).Port;
            listener.Stop();
            return port;
        }

        public void StopDaemon()
        {
            Contract.Invariant( myDaemon != null, "Daemon not started" );

            myDaemon.Stop();
            myComposer.Dispose();
            myHomePageUri = null;
        }

        public bool IsDaemonRunning
        {
            get
            {
                return myDaemon != null;
            }
        }

        internal void CreateNewProject( Project project )
        {
            if( Directory.Exists( project.PagesRoot ) )
            {
                Directory.Delete( project.PagesRoot, true );
            }

            Directory.CreateDirectory( project.PagesRoot );

            var fs = new FileSystemImpl();
            var dest = fs.Directory( project.PagesRoot );

            DeployResource( "HomePage.bwi", dest );
            DeployResource( "Page.Header.bwi", dest );
            DeployResource( "Page.Navigation.bwi", dest );
            DeployResource( "style.css", dest );
        }

        private void DeployResource( string resource, IDirectory dest )
        {
            var folder = Path.Combine( Path.GetDirectoryName( GetType().Assembly.Location ), "Resources", "Templates" );

            using( var writer = dest.File( resource ).CreateWriter() )
            {
                using( var reader = new StreamReader( Path.Combine( folder, resource ) ) )
                {
                    while( !reader.EndOfStream )
                    {
                        writer.WriteLine( reader.ReadLine() );
                    }
                }
            }
        }

        internal Uri HomePageUri
        {
            get
            {
                Contract.Requires( myDaemon != null, "Daemon not running" );

                return myHomePageUri;
            }
        }

        internal Uri GetUriFromPath( string uriPath )
        {
            Contract.RequiresNotNullNotEmpty( uriPath, "uriPath" );
            Contract.Requires( myDaemon != null, "Daemon not running" );

            if( Link.IsExternalLink( uriPath ) )
            {
                return new Uri( uriPath );
            }

            var builder = new UriBuilder( myDaemon.DocumentRootUrl );
            builder.Path = uriPath;
            return builder.Uri;
        }

        internal string GetPageNameFromUri( Uri uri )
        {
            Contract.RequiresNotNull( uri, "uri" );
            Contract.Requires( myDaemon != null, "Daemon not running" );

            if( !uri.ToString().StartsWith( myDaemon.DocumentRootUrl, StringComparison.OrdinalIgnoreCase ) )
            {
                // external link
                return uri.ToString();
            }

            if( uri.Equals( myHomePageUri ) )
            {
                return myComposer.Resolve<SiteConfig>( CompositionContractNames.SiteConfig ).HomePageName;
            }

            return uri.AbsolutePath;
        }

        internal bool IsExternalLink( Uri uri )
        {
            return !uri.ToString().StartsWith( myDaemon.DocumentRootUrl, StringComparison.OrdinalIgnoreCase );
        }

        public void Dispose()
        {
            if( WebCore.IsInitialized )
            {
                WebCore.Shutdown();
            }
        }
    }
}
