using System.IO;
using Plainion.Httpd;
using Plainion.Httpd.Views;
using Plainion.Wiki.Http.Views;

namespace Plainion.Wiki.Http
{
    /// <summary/>
    public class BasicHttpController : AdvancedHttpHandler, IViewContext
    {
        private string myDocumentRoot;
        private IEngine myEngine;

        /// <summary/>
        public BasicHttpController( IEngine engine, string documentRoot )
        {
            myDocumentRoot = documentRoot;
            myEngine = engine;

            ErrorHandler = new ErrorHandler( myEngine );
            Toolbar = GetToolbar();

            BuildViewChain();
        }

        /// <summary/>
        public string Toolbar
        {
            get;
            private set;
        }

        private string GetToolbar()
        {
            var toolbarFile = Path.Combine( myDocumentRoot, "toolbar.html" );
            if ( File.Exists( toolbarFile ) )
            {
                return File.ReadAllText( toolbarFile );
            }

            return string.Empty;
        }
        
        private void BuildViewChain()
        {
            ViewChain.Push( new SearchResultPageView( this ) );
            ViewChain.Push( new ShowPageView( this ) );
            ViewChain.Push( new NewPageView( this, Toolbar ) );
            ViewChain.Push( new CreatePageView( this ) );
            ViewChain.Push( new EditPageView( this, Toolbar ) );
            ViewChain.Push( new UpdatePageView( this ) );

            if ( myDocumentRoot != null )
            {
                ViewChain.Push( new StaticFileView( myDocumentRoot ) );
            }
        }

        /// <summary/>
        IEngine IViewContext.Engine
        {
            get { return myEngine; }
        }
    }
}
