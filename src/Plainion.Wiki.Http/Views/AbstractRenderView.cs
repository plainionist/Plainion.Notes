using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Plainion.Httpd.Views;
using Plainion.Httpd;
using System.Web;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Http.Views
{
    /// <summary/>
    public abstract class AbstractRenderView : AbstractHttpView
    {
        /// <summary/>
        public AbstractRenderView( IViewContext context )
        {
            Context = context;
        }

        /// <summary/>
        protected IViewContext Context
        {
            get;
            private set;
        }

        /// <summary/>
        public override Plainion.Httpd.HttpResponse HandleRequest( HttpListenerRequest request )
        {
            PostContent = request.ParsePostContent().ToDictionary();

            return null;
        }

        /// <summary/>
        protected IDictionary<string, string> PostContent
        {
            get;
            private set;
        }

        /// <summary/>
        protected string GetAction( HttpListenerRequest request )
        {
            var args = request.QueryString;
            return args.AllKeys.Contains( "action" ) ? args[ "action" ] : null;
        }

        /// <summary/>
        protected PageName GetPageName( HttpListenerRequest request )
        {
            var pageUrl = HttpUtility.UrlDecode( request.Url.AbsolutePath );
            pageUrl = pageUrl.Trim();

            if ( string.IsNullOrEmpty( pageUrl ) || pageUrl == "/" )
            {
                pageUrl = Context.Engine.Config.HomePageName;
            }

            if ( pageUrl.EndsWith( "/" ) )
            {
                pageUrl += Context.Engine.Config.NamespaceDefaultPageName;
            }

            return PageName.CreateFromPath( pageUrl );
        }

        /// <summary/>
        protected IEnumerable<string> GetPageContentFromPostRequest( HttpListenerRequest request )
        {
            using ( var reader = new StringReader( PostContent[ "text" ] ) )
            {
                while ( reader.Peek() > 0 )
                {
                    var rawLine = reader.ReadLine();
                    yield return Encoding.UTF8.GetString( request.ContentEncoding.GetBytes( rawLine ) );
                }
            }
        }

        /// <summary/>
        protected string GetPostEditRedirectLocation( PageName pageName )
        {
            var redirectLocation = pageName.FullName;

            if ( PostContent.Keys.OfType<string>().Contains( "SaveAndEdit" ) )
            {
                redirectLocation = pageName.FullName + "?action=edit";
            }

            return redirectLocation;
        }
    }
}
