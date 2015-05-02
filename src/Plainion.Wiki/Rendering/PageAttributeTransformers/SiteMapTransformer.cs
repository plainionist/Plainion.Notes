using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using Plainion.Wiki.Query;
using Plainion.Collections;

namespace Plainion.Wiki.Rendering.PageAttributeTransformers
{
    /// <summary/>
    [PageAttributeTransformer( "sitemap" )]
    public class SiteMapTransformer : IPageAttributeTransformer
    {
        /// <summary/>
        public void Transform( PageAttribute pageAttribute, EngineContext context )
        {
            var pages = context.Query.All().ToList();

            var siteMap = BuildSiteMap( pages );

            pageAttribute.Parent.ReplaceChild( pageAttribute, siteMap );
        }

        private PageLeaf BuildSiteMap( IEnumerable<PageName> pages )
        {
            if ( !pages.Any() )
            {
                return new Content();
            }

            var list = new BulletList();

            var pageTree = pages.OrderBy( p => p.Namespace.AsPath ).ToQueue();
            BuildSubTree( list, PageNamespace.Empty, pageTree );

            return list;
        }

        // assumes list is sorted by namespace
        private void BuildSubTree( BulletList list, PageNamespace ns, Queue<PageName> pages )
        {
            while ( pages.Any() )
            {
                var page = pages.Peek();

                if ( page.Namespace == ns )
                {
                    // add to the list
                    page = pages.Dequeue();

                    list.Consume( new ListItem( new TextBlock( new Link( page.FullName, page.Name ) ) ) );

                    continue;
                }

                if ( page.Namespace.StartsWith( ns ) )
                {
                    // one step into recursion

                    // headline of this subtree
                    list.Consume( new ListItem( new TextBlock( page.Namespace.Elements.Last() ) ) );

                    // build subtree
                    var subList = new BulletList();
                    BuildSubTree( subList, page.Namespace, pages );
                    list.Consume( subList );
                }
                else
                {
                    // go one step back/up from recursion
                    return;
                }
            }
        }
    }
}
