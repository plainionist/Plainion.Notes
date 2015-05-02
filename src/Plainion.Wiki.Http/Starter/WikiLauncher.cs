using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Plainion.IO;
using Plainion.IO.RealFS;
using Plainion.Logging;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.DataAccess.FlatFile;
using Plainion.Wiki.Html;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Http;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;
using Plainion.Composition;
using Plainion.AppFw.Shell.Forms;

namespace Plainion.Wiki.Http.Starter
{
    public class WikiLauncher : FormsAppBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( WikiLauncher ) );

        [Argument( Short = "-p", Long = "-port", Description = "Server port" )]
        public int Port
        {
            get;
            set;
        }

        [Required]
        [Argument( Short = "-d", Long = "-docroot", Description = "Document root" )]
        public string DocumentRoot
        {
            get;
            set;
        }

        protected override void Run()
        {
            if ( !Directory.Exists( DocumentRoot ) )
            {
                throw new FileNotFoundException( "DocumentRoot does not exist", DocumentRoot );
            }

            var composer = new Composer();

            var fs = new FileSystemImpl();
            composer.RegisterInstance<IFileSystem>( fs );
            composer.RegisterInstance( CompositionContractNames.FileSystemRoot, fs.Directory( DocumentRoot ) );

            composer.Register(
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

            composer.Register( typeof( WikiServer ) );

            var decoratorCatalog = new DecoratorChainCatalog( typeof( IPageAccess ) );
            decoratorCatalog.Add( typeof( FlatFilePageAccess ) );
            decoratorCatalog.Add( typeof( AuditingPageAccessDecorator ) );
            composer.Add( decoratorCatalog );

            composer.RegisterRenderActions( typeof( HtmlRenderer ).Assembly );
            composer.RegisterRenderingSteps( typeof( Engine ).Assembly );
            composer.RegisterPageAttributeTransformers( typeof( Engine ).Assembly );

            var serverSite = new DefaultServerSite( DocumentRoot );
            composer.RegisterInstance<IServerSite>( serverSite );

            composer.Compose();

            var server = composer.Resolve<WikiServer>();
            server.Start();

            Console.WriteLine( "Plainion.Wiki server listening to {0} ... press ^C to stop", server.DocumentRootUrl );
        }
    }
}
