using Plainion.Testing;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.IntegrationTests.Tasks;
using Plainion.Wiki.Resources;
using NUnit.Framework;

namespace Plainion.Wiki.IntegrationTests
{
    [TestFixture]
    public class RenderingTests : TestBase
    {
        [Test]
        public void RenderEmptyPageWithoutStylesheet()
        {
            var pageName = Engine.CreatePage( "Page1" );

            var output = Engine.Render( pageName );

            var expected = LoadTestData( "Rendering_RenderEmptyPage_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void RenderSimplePage()
        {
            var stylesheet= Composer.Resolve<HtmlStylesheet>();
            stylesheet.ExternalStylesheet = ResourceNames.CssStylesheet;
            stylesheet.ExternalJavascript = ResourceNames.JavaScript;

            var pageName = Engine.CreatePage( "Page1",
                "!!! Welcome",
                string.Empty,
                "to integration tests" );

            var output = Engine.Render( pageName );

            var expected = LoadTestData( "Rendering_RenderSimplePage_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }


        [Test,Ignore("Currently not supported. Conflicting requirements")]
        // Details for ignore: what if we have edit attribute in header? then we want the page and not the body to be
        // referenced.
        public void RenderPageAttributesInSideBar()
        {
            var sidebar = Engine.CreatePage( ResourceNames.PageSideBar, "[@page.edit]" );
            var pageName = Engine.CreatePage( "Page1" );

            var output = Engine.Render( pageName );

            var expected = LoadTestData( "Rendering_RenderPageAttributesInSideBar_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }
    }
}
