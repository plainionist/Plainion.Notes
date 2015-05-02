using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class LinkRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlLinkWritten()
        {
            var link = new Link( "http://www.google.de", "Google" );
            var renderAction = new LinkRenderAction();

            Render( renderAction, link );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<a href=\"http://www.google.de\">Google</a>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
