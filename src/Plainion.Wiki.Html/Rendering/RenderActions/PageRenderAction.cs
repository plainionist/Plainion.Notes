using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( Page ) )]
    public class PageRenderAction : GenericRenderAction<Page, IHtmlRenderActionContext>
    {
        /// <summary/>
        protected override void Render( Page page )
        {
            WriteLine( " <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>" );
            WriteLine( " <html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en' lang='en'>" );
            WriteLine( "   <head lang='en'>" );

            Write( "       <title>" );
            var pageTitle = Context.RenderingContext.EngineContext.Config.StaticPageTitle ?? page.Name.Name;
            Write( pageTitle );
            WriteLine( "       </title>" );

            WriteLine( "      <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" );

            if ( Context.Stylesheet.ExternalStylesheet != null )
            {
                Write( "      <link rel='stylesheet' type='text/css' media='screen' href='/" );
                Write( Context.Stylesheet.ExternalStylesheet );
                WriteLine( "' />" );
            }

            if ( Context.Stylesheet.ExternalJavascript != null )
            {
                Write( "      <script type='text/javascript' src='/" );
                Write( Context.Stylesheet.ExternalJavascript );
                WriteLine( "'></script>" );
            }

            WriteLine( "      <style type='text/css'>" );
            WriteLine( "          /* dirty hack for IE6. */" );
            WriteLine( "           #quickbar {" );
            WriteLine( "           position: absolute;" );
            WriteLine( "           }" );
            WriteLine( "      </style>" );
            WriteLine( "   </head>" );

            if ( page.Content.Type == PageBodyType.Content )
            {
                WriteLine( "   <script type='text/javascript' language='JavaScript'>" );
                WriteLine( "       function edit() {" );
                WriteLine( "           location.href = '?action=edit';" );
                WriteLine( "       }" );
                WriteLine( "   </script>" );
                WriteLine( "   <body ondblclick='edit()'>" );
            }
            else
            {
                WriteLine( "   <body>" );
            }

            WriteLine( "       <div id='scrolling'>" );
            WriteLine( "           <div id='header'>" );

            if ( page.Header != null )
            {
                RenderChildrenWithoutOuterParagraph( page.Header );
            }

            WriteLine( "           </div>" );
            WriteLine( "           <div id='content'>" );

            if ( Context.RenderingContext.EngineContext.Config.RenderPageNameAsHeadline )
            {
                Write( "<h1 id='pagetitle'>" );
                Write( page.Name.Name );
                WriteLine( "</h1>" );
            }

            if ( page.Content != null )
            {
                Render( page.Content.Children );
            }

            WriteLine( "           </div>" );

            if ( page.Footer != null )
            {
                WriteLine( "           <div id='footer'>" );
                RenderChildrenWithoutOuterParagraph( page.Footer );
                WriteLine( "          </div>" );
            }

            WriteLine( "       </div>" );
            WriteLine( "       <div id='sidebar'>" );

            if ( page.SideBar != null )
            {
                RenderChildrenWithoutOuterParagraph( page.SideBar );
            }

            WriteLine( "       </div>" );
            WriteLine( "   </body>" );
            WriteLine( "   </html>" );
        }

        // if real content has surrounding paragraph if will be omitted
        private void RenderChildrenWithoutOuterParagraph( PageNode node )
        {
            var content = node.Children;

            if ( content.Count() == 1 && content.First() is Paragraph )
            {
                var para = (Paragraph)content.First();
                content = para.Children;
            }

            Render( content );
        }
    }
}
