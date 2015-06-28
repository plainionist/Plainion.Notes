using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion.Composition;
using Plainion.IO;
using Plainion.IO.MemoryFS;
using Plainion.Prism.Events;
using Plainion.Wiki;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.DataAccess.FlatFile;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.Xaml;
using Plainion.Wiki.Xaml.Rendering;

namespace Plainion.Notes.Services
{
    [Export]
    public class WikiService
    {
        private IDirectory myFileSystemRoot;
        private FileSystemImpl myFileSystem;
        private IEngine myEngine;

        [ImportingConstructor]
        public WikiService( IEventAggregator eventAggregator )
        {
            HomePage = PageName.Create( "Home" );

            eventAggregator.GetEvent<ApplicationShutdownEvent>().Subscribe( OnShutdown );
            var composer = new Composer();

            bool createStartPage = true;

            var fsBlob = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Plainion", "Notes", "fs.bin" );
            if( File.Exists( fsBlob ) )
            {
                using( var stream = new FileStream( fsBlob, FileMode.Open, FileAccess.Read ) )
                {
                    myFileSystem = FileSystemImpl.Deserialize( stream );
                }

                createStartPage = false;
            }
            else
            {
                myFileSystem = new FileSystemImpl();
            }

            myFileSystemRoot = myFileSystem.Directory( "Z:" );

            composer.RegisterInstance<IFileSystem>( myFileSystem );
            composer.RegisterInstance( CompositionContractNames.FileSystemRoot, myFileSystemRoot );

            composer.Register(
                typeof( DefaultFlatFileCompositionDescriptor ),
                typeof( RenderingPipeline ),
                typeof( RenderingStepCatalog ),
                typeof( PageAttributeTransformerCatalog ),
                typeof( QueryCompiler ),
                typeof( Engine ),
                typeof( FlatFilePageAccess ),
                typeof( NullPageHistoryAccess ),
                typeof( PageRepository ),
                typeof( ParserPipeline ),
                typeof( XamlRenderer ),
                typeof( XamlRenderActionCatalog ),
                typeof( DefaultErrorPageHandler ),
                typeof( NullAuditingLog )
            );

            composer.RegisterRenderActions( typeof( XamlRenderer ).Assembly );
            composer.RegisterRenderingSteps( typeof( Engine ).Assembly );
            composer.RegisterPageAttributeTransformers( typeof( Engine ).Assembly );

            composer.Compose();

            var siteConfig = composer.Resolve<SiteConfig>( CompositionContractNames.SiteConfig );
            siteConfig.RenderPageNameAsHeadline = false;

            myEngine = composer.Resolve<IEngine>();

            if( createStartPage )
            {
                myEngine.Create( HomePage, new[] { "! Welcome", "", "Put ur notes here :)" } );
            }

            InitPagesIndex();
        }

        private void InitPagesIndex()
        {
            var pagesIdx = myFileSystemRoot.File( "Pages.idx" );
            if( pagesIdx.Exists )
            {
                var pages = pagesIdx.ReadAllLines()
                    .Select( pagePath => PageName.CreateFromPath( pagePath ) );

                Pages = new ObservableCollection<PageName>( pages );
            }
            else
            {
                Pages = new ObservableCollection<PageName> { HomePage };
            }
        }

        public PageName HomePage { get; private set; }

        public ObservableCollection<PageName> Pages { get; private set; }

        public FlowDocument Render( PageName page )
        {
            using( var stream = new MemoryStream() )
            {
                myEngine.Render( page, stream );

                stream.Flush();
                stream.Seek( 0, SeekOrigin.Begin );

                var xml = Encoding.Default.GetString( stream.ToArray() );

                return ( FlowDocument )XamlReader.Parse( xml );
            }
        }

        public string[] GetPageText( PageName page )
        {
            return myEngine.Find( page ).GetContent();
        }

        public void Save( PageName page, string content )
        {
            if( myEngine.Find( page ) != null )
            {
                myEngine.Update( page, content.Split( new[] { Environment.NewLine }, StringSplitOptions.None ) );
            }
            else
            {
                myEngine.Create( page, content.Split( new[] { Environment.NewLine }, StringSplitOptions.None ) );
                Pages.Add( page );
            }

            // ensure that we do not lose data
            FlushFileSystem();
        }

        public void Delete( PageName page )
        {
            myEngine.Delete( page );
            Pages.Remove( page );
        }

        private void OnShutdown( object obj )
        {
            FlushFileSystem();
        }

        private void FlushFileSystem()
        {
            var pagesIdx = myFileSystemRoot.File( "Pages.idx" );
            pagesIdx.WriteAll( Pages.Select( p => p.FullName ).ToArray() );

            var appDir = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), "Notes.db" );
            if( !Directory.Exists( appDir ) )
            {
                Directory.CreateDirectory( appDir );
            }

            var fsBlob = Path.Combine( appDir, "fs.bin" );
            using( var stream = new FileStream( fsBlob, FileMode.Create, FileAccess.Write ) )
            {
                myFileSystem.Serialize( stream );
            }
        }
    }
}
