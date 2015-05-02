using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using System.Linq.Expressions;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Provides a generic and a semantical API to execute queries on the given <see cref="PageRepository"/>
    /// </summary>
    public class QueryEngine
    {
        private PageRepository myPageRepository;

        /// <summary/>
        public QueryEngine( PageRepository pageRepository )
        {
            if ( pageRepository == null )
            {
                throw new ArgumentNullException( "pageRepository" );
            }

            myPageRepository = pageRepository;
        }

        /// <summary/>
        public IEnumerable<QueryMatch> Where( IQueryMatcher matcher )
        {
            return myPageRepository.Pages
                .Select( descriptor => new PageHandle( descriptor, myPageRepository.Get( descriptor ) ) )
                .SelectMany( page => matcher.Match( page ) )
                .ToList();
        }

        /// <summary/>
        public IEnumerable<QueryMatch> PageContains( string searchText )
        {
            return Where( new FulltextMatcher( searchText ) );
        }

        /// <summary/>
        public IEnumerable<QueryMatch> ReferencingPages( PageName pageName )
        {
            return Where( new ReferencesPageMatcher( pageName ) );
        }

        /// <summary/>
        public IEnumerable<PageName> All()
        {
            return myPageRepository.Pages
                .Select( p => p.Name )
                .ToList();
        }
    }
}
