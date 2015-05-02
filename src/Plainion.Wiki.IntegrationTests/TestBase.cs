using System.IO;
using Plainion.IO;
using Plainion.IO.RealFS;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.DataAccess.FlatFile;
using Plainion.Wiki.IntegrationTests.Tasks;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;
using NUnit.Framework;
using Plainion.Testing;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Html;
using Plainion.Composition;

namespace Plainion.Wiki.IntegrationTests
{
    public class TestBase
    {
        private Composer myComposer;
        private IEngine myEngine;
        private DocumentRoot myDocumentRoot;

        protected IEngine Engine
        {
            get
            {
                if ( myEngine == null )
                {
                    myEngine = myComposer.Resolve<IEngine>();
                }

                return myEngine;
            }
        }

        protected IComposer Composer
        {
            get
            {
                return myComposer;
            }
        }

        protected DocumentRoot DocumentRoot
        {
            get
            {
                return myDocumentRoot;
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            var fs = new FileSystemImpl();

            myDocumentRoot = new DocumentRoot( fs );
            myDocumentRoot.Create();

            myComposer = new Composer();

            myComposer.RegisterInstance<IFileSystem>( fs );
            myComposer.RegisterInstance( CompositionContractNames.FileSystemRoot, myDocumentRoot.Directory );

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
                typeof( ThrowErrorPageHandler ) 
            );

            var decoratorCatalog = new DecoratorChainCatalog( typeof( IPageAccess ) );
            decoratorCatalog.Add( typeof( FlatFilePageAccess ) );
            decoratorCatalog.Add( typeof( AuditingPageAccessDecorator ) );
            myComposer.Add( decoratorCatalog );

            myComposer.RegisterRenderActions( typeof( HtmlRenderer ).Assembly );
            myComposer.RegisterRenderingSteps( typeof( Engine ).Assembly );
            myComposer.RegisterPageAttributeTransformers( typeof( Engine ).Assembly );

            myComposer.Compose();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if ( myDocumentRoot != null )
            {
                myDocumentRoot.Delete();
            }

            myEngine = null;
        }

        protected string[] LoadTestData( string name )
        {
            var file = this.TestEnvironment().GetTestResource( name );

            return File.ReadAllLines( file );
        }
    }
}
