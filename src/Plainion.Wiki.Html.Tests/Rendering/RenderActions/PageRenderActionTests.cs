using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class PageRenderActionTests : TestBase
    {
        [Test]
        public void Render_WithExternalStylesheet_StylesheetIsReferenced()
        {
            Stylesheet.ExternalStylesheet = "@@@ExternalStylesheet@@@";
            var page = CreatePageWithContent();
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.IsTrue( output.Any( line => line.Contains( Stylesheet.ExternalStylesheet ) ),
                "Stylesheet reference missing" );
        }

        [Test]
        public void Render_WithoutExternalStylesheet_NoStylesheetReferenced()
        {
            Stylesheet.ExternalStylesheet = null;
            var page = CreatePageWithContent();
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.IsFalse( output.Any( line => line.Contains( "link" ) && line.Contains( "text/css" ) ),
                "Stylesheet reference found" );
        }

        [Test]
        public void Render_WithExternalJavascript_JavascriptIsReferenced()
        {
            Stylesheet.ExternalJavascript = "@@@ExternalJavascript@@@";
            var page = CreatePageWithContent();
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.IsTrue( output.Any( line => line.Contains( Stylesheet.ExternalJavascript ) ),
                "Javascript reference missing" );
        }

        [Test]
        public void Render_WithoutExternalJavascript_NoJavascriptReferenced()
        {
            Stylesheet.ExternalJavascript = null;
            var page = CreatePageWithContent();
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.IsFalse( output.Any( line => line.Contains( "script" ) &&
                line.Contains( "text/javascript" ) && line.Contains( "src=" ) ),
                "Javascript reference found" );
        }

        [Test]
        public void Render_WhenCalled_PageContentIsRendered()
        {
            var page = CreatePageWithContent();
            OnNestedRenderCall = RenderSubPageBody;
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.That( output, Contains.Item( "@@@content@@@" ) );
        }

        [Test]
        public void Render_PageTypeIsContent_EditWithDoubleClickIsEnabled()
        {
            var page = CreatePageWithContent();
            page.Content.Type = PageBodyType.Content;
            OnNestedRenderCall = RenderSubPageBody;
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.IsTrue( output.Any( line => line.Contains( "<body" ) && line.Contains( "ondblclick" ) ),
                "edit function found" );
        }

        [Test]
        public void Render_PageTypeIsEdit_EditWithDoubleClickIsDisabled()
        {
            var page = CreatePageWithContent();
            page.Content.Type = PageBodyType.Edit;
            OnNestedRenderCall = RenderSubPageBody;
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.IsFalse( output.Any( line => line.Contains( "<body" ) && line.Contains( "ondblclick" ) ),
                "edit function found" );
        }

        [Test]
        public void Render_WithHeader_HeaderIsRendered()
        {
            var page = CreatePageWithContent();
            page.Header = new PageBody( PageName.Create( "header" ), new PlainText( "a" ) );
            OnNestedRenderCall = RenderSubPageBody;
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.That( output, Contains.Item( "@@@header@@@" ) );
        }

        [Test]
        public void Render_Withsidebar_sidebarIsRendered()
        {
            var page = CreatePageWithContent();
            page.SideBar = new PageBody( PageName.Create( "sidebar" ), new PlainText( "a" ) );
            OnNestedRenderCall = RenderSubPageBody;
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.That( output, Contains.Item( "@@@sidebar@@@" ) );
        }

        [Test]
        public void Render_Withfooter_footerIsRendered()
        {
            var page = CreatePageWithContent();
            page.Footer = new PageBody( PageName.Create( "footer" ), new PlainText( "a" ) );
            OnNestedRenderCall = RenderSubPageBody;
            var renderAction = new PageRenderAction();

            Render( renderAction, page );

            var output = GetRenderingOutput().ToList();
            Assert.That( output, Contains.Item( "@@@footer@@@" ) );
        }

        private Page CreatePageWithContent()
        {
            var page = new Page( PageName.Create( "a" ) );
            page.Content = new PageBody( PageName.Create( "content" ), new PlainText( "body" ) );
            return page;
        }

        private void RenderSubPageBody( PageLeaf node )
        {
            var body = node.GetParentOfType<PageBody>();
            RenderingContext.Writer.WriteLine( "@@@" + body.Name.Name + "@@@" );
        }
    }
}
