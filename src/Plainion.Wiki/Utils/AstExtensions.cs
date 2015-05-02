using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Utils
{
    /// <summary>
    /// Convenience and utility functions to work with AST.
    /// </summary>
    public static class AstExtensions
    {
        /// <summary/>
        public static Headline FindRelatedHeadline( this PageLeaf node )
        {
            PageNode parent = node.Parent;
            PageLeaf childICameFrom = node;
            Headline headline = null;

            while ( headline == null && parent != null )
            {
                var childrenBeforeMe = parent.Children
                    .TakeWhile( child => child != childICameFrom );

                headline = childrenBeforeMe
                    .OfType<Headline>()
                    .LastOrDefault();

                childICameFrom = parent;
                parent = parent.Parent;
            }

            return headline;
        }

        /// <summary/>
        public static bool IsLinkingPage( this Link link, PageName pageName )
        {
            if ( link.IsExternal )
            {
                return false;
            }

            var url = link.UrlWithoutAnchor;
            if ( url.StartsWith( "/" ) )
            {
                // URL is rooted so we can directly compare with PageName.FullName
                return url == pageName.FullName;
            }

            if ( pageName.Namespace.IsEmpty )
            {
                // PageName is rooted so check without namespace
                var linkedPageName = PageName.Create( url );
                return pageName == linkedPageName;
            }

            // PageName is not rooted so check with namespace of the current page
            var body = link.GetParentOfType<PageBody>();
            if ( body == null )
            {
                // it is a relative link without connection to a page
                // so we cannot determine for sure whether it is referencing
                // the given page
                return false;
            }

            {
                var linkedPageName = PageName.Create( body.Name.Namespace, url );
                return pageName == linkedPageName;
            }
        }

        /// <summary>
        /// Returns the whole tree of nodes (the given one and all its children 
        /// and sub-children) in a flat list.
        /// </summary>
        public static IEnumerable<PageLeaf> GetFlattenedTree( this PageNode node )
        {
            var finder = new AstFinder<PageLeaf>( leaf => true );
            return finder.Where( node );
        }

        /// <summary>
        /// Returns the "real" name of the page. 
        /// If the root of the given node is a <see cref="PageBody"/> the name of the 
        /// PageBody is returned. If the root of the given node is a 
        /// <see cref="Page"/> the name of the Page is returned. Otherwise
        /// null is returned.
        /// </summary>
        public static PageName GetNameOfPage( this PageLeaf node )
        {
            var root = node.GetRoot();

            var page = root as Page;
            if ( page != null )
            {
                return page.Name;
            }

            var body = root as PageBody;
            if ( body != null )
            {
                return body.Name;
            }

            return null;
        }
    }
}
