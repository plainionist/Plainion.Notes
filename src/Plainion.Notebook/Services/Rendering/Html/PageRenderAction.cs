using System.ComponentModel.Composition;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Rendering;

namespace Plainion.Notebook.Services.Rendering.Html
{
    [HtmlRenderAction(typeof(Page))]
    class PageRenderAction : GenericRenderAction<Page, IHtmlRenderActionContext>
    {
        private string myClientScriptsRoot;
        private WikiMetadata myWikiMetadata;

        [ImportingConstructor]
        public PageRenderAction([Import(CompositionContracts.ClientScriptsRootUrl)]string clientScriptsRoot, WikiMetadata wikiMetadata)
        {
            myClientScriptsRoot = clientScriptsRoot;
            myWikiMetadata = wikiMetadata;
        }

        protected override void Render(Page page)
        {
            WriteLine(" <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>");
            WriteLine(" <html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en' lang='en'>");
            WriteLine("   <head lang='en'>");

            Write("       <title>");
            var pageTitle = Context.RenderingContext.EngineContext.Config.StaticPageTitle ?? page.Name.Name;
            Write(pageTitle);
            WriteLine("       </title>");

            WriteLine("      <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>");

            Write("      <link rel='stylesheet' type='text/css' media='screen' href='");
            Write(myClientScriptsRoot);
            Write("/jquery.textcomplete-0.4.0.css' />");

            Write("      <script type='text/javascript' src='");
            Write(myClientScriptsRoot);
            Write("/jquery-1.11.3.min.js'></script>");

            Write("      <script type='text/javascript' src='");
            Write(myClientScriptsRoot);
            Write("/jquery-textcomplete-0.4.0.min.js'></script>");

            if (Context.Stylesheet.ExternalStylesheet != null)
            {
                Write("      <link rel='stylesheet' type='text/css' media='screen' href='/");
                Write(Context.Stylesheet.ExternalStylesheet);
                WriteLine("' />");
            }

            if (Context.Stylesheet.ExternalJavascript != null)
            {
                Write("      <script type='text/javascript' src='/");
                Write(Context.Stylesheet.ExternalJavascript);
                WriteLine("'></script>");
            }

            WriteLine("      <style type='text/css'>");
            WriteLine("          /* dirty hack for IE6. */");
            WriteLine("           #quickbar {");
            WriteLine("           position: absolute;");
            WriteLine("           }");
            WriteLine("      </style>");
            WriteLine("   </head>");

            var cssClass = myWikiMetadata.IsTool(page.Name) ? "tool" : "page";

            if (page.Content.Type == PageBodyType.Content)
            {
                WriteLine("   <script type='text/javascript' language='JavaScript'>");
                WriteLine("       function edit() {");
                WriteLine("           location.href = '?action=edit';");
                WriteLine("       }");
                WriteLine("   </script>");
                WriteLine("   <body class='" + cssClass + "' ondblclick='edit()'>");
            }
            else
            {
                Write("      <script type='text/javascript' src='");
                Write(myClientScriptsRoot);
                Write("/main.js'></script>");

                WriteLine("   <body class='" + cssClass + "'>");

                WriteLine("<datalist id='pages'>");
                var contentPages = Context.RenderingContext.EngineContext.Query.All()
                    .Where(n => n != page.Name)
                    .Where(n => myWikiMetadata.IsContent(n));
                foreach (var name in contentPages)
                {
                    Write("<option>");
                    Write(name.Name);
                    Write("</option>");
                }
                WriteLine("</datalist>");
            }

            WriteLine("       <div id='header'>");

            if (page.Header != null)
            {
                RenderChildrenWithoutOuterParagraph(page.Header);
            }

            WriteLine("       </div>");
            WriteLine("       <div id='content'>");

            if (Context.RenderingContext.EngineContext.Config.RenderPageNameAsHeadline)
            {
                Write("<h1>");
                Write(page.Name.Name);
                WriteLine("</h1>");
            }

            if (page.Content != null)
            {
                Render(page.Content.Children);
            }

            WriteLine("       </div>");

            if (page.Footer != null)
            {
                WriteLine("       <div id='footer'>");
                RenderChildrenWithoutOuterParagraph(page.Footer);
                WriteLine("      </div>");
            }

            WriteLine("   </body>");
            WriteLine("   </html>");
        }

        // if real content has surrounding paragraph if will be omitted
        private void RenderChildrenWithoutOuterParagraph(PageNode node)
        {
            var content = node.Children;

            if (content.Count() == 1 && content.First() is Paragraph)
            {
                var para = (Paragraph)content.First();
                content = para.Children;
            }

            Render(content);
        }
    }
}
