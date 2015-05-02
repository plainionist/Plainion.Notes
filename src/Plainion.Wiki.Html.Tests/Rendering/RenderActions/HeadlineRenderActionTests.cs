using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class HeadlineRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlHeadlineWritten()
        {
            var headline = new Headline( "a", 1 );
            var renderAction = new HeadlineRenderAction();

            Render( renderAction, headline );

            var output = GetRenderingOutput();
            Assert.That( output.Single(), Contains.Substring( "<h1>a</h1>" ) );
        }

        [Test]
        public void Render_WhenCalled_AnchorCreatedForHeadline()
        {
            var headline = new Headline( "Some text", 3 );
            var renderAction = new HeadlineRenderAction();

            Render( renderAction, headline );

            var output = GetRenderingOutput();
            Assert.That( output.Single(), Contains.Substring( "<a name=\"Sometext\">" ) );
        }
    }
}
