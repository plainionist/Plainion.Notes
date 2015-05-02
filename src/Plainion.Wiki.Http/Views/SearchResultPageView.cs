using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Plainion.Httpd.Views;
using Plainion.Httpd;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Query;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Http.Views
{
    /// <summary/>
    public class SearchResultPageView : AbstractRenderView
    {
        /// <summary/>
        public SearchResultPageView( IViewContext context )
            : base( context )
        {
        }

        /// <summary/>
        public override bool CanHandleRequest( HttpListenerRequest request )
        {
            return GetAction( request ) == "search";
        }

        /// <summary/>
        public override HttpResponse HandleRequest( HttpListenerRequest request )
        {
            var response = new HttpResponse();

            var searchText = request.QueryString[ "text" ];
            var hits = Context.Engine.Query.PageContains( searchText );
            var page = CreateSearchResultPage( searchText, hits );

            Context.Engine.Render( page, response.OutputStream );
            response.OutputStream.Close();

            return response;
        }

        private PageBody CreateSearchResultPage( string searchText, IEnumerable<QueryMatch> hits )
        {
            var page = new PageBody( PageName.Create( PageNames.SiteSearchResults ) );
            page.Consume( new Headline( "Search results for '" + searchText + "'", 3 ) );

            page.Consume( ContentBuilder.BuildQueryResult( hits, "no results" ) );

            return page;
        }
    }
}
