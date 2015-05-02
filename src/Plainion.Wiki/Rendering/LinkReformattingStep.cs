using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using System.Xml.Linq;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Reformats the abstract link description of the AST into a definitive form for the renderer.
    /// </summary>
    /// <remarks>
    /// E.g. transforms a link to a non-existing page into a link to create the page
    /// </remarks>
    [RenderingStep( RenderingStage.AttributePreProcessing + 1 )]
    public class LinkReformattingStep : IRenderingStep
    {
        private EngineContext myContext;

        /// <summary/>
        public PageLeaf Transform( PageLeaf node, EngineContext context )
        {
            try
            {
                myContext = context;

                var walker = new AstWalker<Link>( ReformatLink );
                walker.Visit( node );

                return node;
            }
            finally
            {
                myContext = null;
            }
        }

        private void ReformatLink( Link link )
        {
            var reformattedLink = GetDefinitiveLink( link );

            link.Parent.ReplaceChild( link, reformattedLink );
        }

        private PageLeaf GetDefinitiveLink( Link link )
        {
            var url = GetUrlIfExists( link );
            if ( url == null )
            {
                return new Content(
                    new PlainText( link.Text ),
                    new Link( link.Url + "?action=new", "?" ) );
            }

            return new Link( url, link.Text );
        }

        private string GetUrlIfExists( Link link )
        {
            if ( link.IsExternal || link.IsStatic )
            {
                return link.Url;
            }

            // check if page exists
            var ns = GetPageNamespace( link );
            var pageName = myContext.FindPageByName( ns, link.UrlWithoutAnchor );

            // TODO: returning null here allows us to reference anchors on local page with [.#anchor|more]
            //       this implicit behavior should be made explicit
            var url = pageName != null ? pageName.FullName : null;
            if ( !string.IsNullOrEmpty( link.Anchor ) )
            {
                url += "#" + link.Anchor;
            }

            return url;
        }

        private PageNamespace GetPageNamespace( Link link )
        {
            var body = link.GetParentOfType<PageBody>();
            return body != null ? body.Name.Namespace : null;
        }
    }
}
