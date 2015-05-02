using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Http;
using Plainion.Wiki.Http.Views;
using Plainion.Wiki.Rendering;

namespace Plainion.Notebook.Rendering.Html
{
    [HtmlRenderAction( typeof( Page ) )]
    public class PageRenderAction : GenericRenderAction<Page, IHtmlRenderActionContext>
    {
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

            if( Context.Stylesheet.ExternalStylesheet != null )
            {
                Write( "      <link rel='stylesheet' type='text/css' media='screen' href='/" );
                Write( Context.Stylesheet.ExternalStylesheet );
                WriteLine( "' />" );
            }

            if( Context.Stylesheet.ExternalJavascript != null )
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

            var cssClass = IsTool( page ) ? "tool" : "page";

            if( page.Content.Type == PageBodyType.Content )
            {
                WriteLine( "   <script type='text/javascript' language='JavaScript'>" );
                WriteLine( "       function edit() {" );
                WriteLine( "           location.href = '?action=edit';" );
                WriteLine( "       }" );
                WriteLine( "   </script>" );
                WriteLine( "   <body class='" + cssClass + "' ondblclick='edit()'>" );
            }
            else
            {
                WriteLine( "   <body class='" + cssClass + "'>" );
            }

            WriteLine( "       <div id='header'>" );

            if( page.Header != null )
            {
                RenderChildrenWithoutOuterParagraph( page.Header );
            }

            WriteLine( "       </div>" );
            WriteLine( "       <div id='content'>" );

            if( Context.RenderingContext.EngineContext.Config.RenderPageNameAsHeadline )
            {
                Write( "<h1>" );
                Write( page.Name.Name );
                WriteLine( "</h1>" );
            }

            if( page.Content != null )
            {
                Render( page.Content.Children );
            }

            WriteLine( "       </div>" );

            if( page.Footer != null )
            {
                WriteLine( "       <div id='footer'>" );
                RenderChildrenWithoutOuterParagraph( page.Footer );
                WriteLine( "      </div>" );
            }

            WriteLine( "   </body>" );
            WriteLine( "   </html>" );
        }

        private static bool IsTool( Page page )
        {
            return page.Name.Name == "Page.Navigation"
                || page.Name.Name == PageNames.SiteSearchResults;
        }

        // if real content has surrounding paragraph if will be omitted
        private void RenderChildrenWithoutOuterParagraph( PageNode node )
        {
            var content = node.Children;

            if( content.Count() == 1 && content.First() is Paragraph )
            {
                var para = ( Paragraph )content.First();
                content = para.Children;
            }

            Render( content );
        }
    }
}
