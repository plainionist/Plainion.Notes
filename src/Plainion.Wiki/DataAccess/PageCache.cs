using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Caches parsed pages.
    /// </summary>
    public class PageCache
    {
        private Dictionary<PageName, PageBody> myPageNameToPageMap;

        /// <summary/>
        public PageCache()
        {
            myPageNameToPageMap = new Dictionary<PageName, PageBody>();
        }

        /// <summary/>
        public IEnumerable<PageName> Pages
        {
            get { return myPageNameToPageMap.Keys; }
        }

        /// <summary/>
        public PageBody FindByName( PageName pageName )
        {
            if ( myPageNameToPageMap.ContainsKey( pageName ) )
            {
                //Console.WriteLine( "PageCache hit: " + pageName );
                return myPageNameToPageMap[ pageName ];
            }

            return null;
        }

        /// <summary>
        /// Adds the given page. Overwrites the cache entry if the page
        /// already exists.
        /// </summary>
        public void Add( PageBody page )
        {
            myPageNameToPageMap[ page.Name ] = page;
        }

        /// <summary/>
        public void Remove( PageName pageName )
        {
            if ( myPageNameToPageMap.ContainsKey( pageName ) )
            {
                myPageNameToPageMap.Remove( pageName );
            }
        }

        /// <summary/>
        public void Clear()
        {
            myPageNameToPageMap.Clear();
        }
    }
}
