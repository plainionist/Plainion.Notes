using System.IO;
using Plainion.Httpd;
using Plainion.Httpd.Views;
using Plainion.Wiki.Http.Views;

namespace Plainion.Wiki.Http
{
    public class BasicHttpController : AdvancedHttpHandler, IViewContext
    {
        private IEngine myEngine;

        public BasicHttpController( IEngine engine )
        {
            myEngine = engine;

            ErrorHandler = new ErrorHandler( myEngine );
        }

        public string DocumentRoot { get; set; }

        public string ClientScriptsRoot { get; set; }

        public void Initialize()
        {
            Contract.RequiresNotNull( ViewChain.Count == 0, "Already initialized" );

            var toolbar = GetToolbarContent();

            ViewChain.Push( new SearchResultPageView( this ) );
            ViewChain.Push( new ShowPageView( this ) );
            ViewChain.Push( new NewPageView( this, toolbar ) );
            ViewChain.Push( new CreatePageView( this ) );
            ViewChain.Push( new EditPageView( this, toolbar ) );
            ViewChain.Push( new UpdatePageView( this ) );

            if( DocumentRoot != null )
            {
                ViewChain.Push( new StaticFileView( DocumentRoot ) );
            }

            if( ClientScriptsRoot != null )
            {
                ViewChain.Push( new StaticFileView( ClientScriptsRoot ) );
            }
        }

        private string GetToolbarContent()
        {
            if( DocumentRoot == null )
            {
                return string.Empty;
            }

            var toolbarFile = Path.Combine( DocumentRoot, "toolbar.html" );
            if( File.Exists( toolbarFile ) )
            {
                return File.ReadAllText( toolbarFile );
            }

            return string.Empty;
        }

        IEngine IViewContext.Engine
        {
            get { return myEngine; }
        }
    }
}
