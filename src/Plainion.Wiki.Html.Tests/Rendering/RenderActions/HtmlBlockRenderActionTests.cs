using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;
using Plainion.Wiki.Html.AST;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class HtmlBlockRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlContentJustWritten()
        {
            var para = new HtmlBlock( "a", "b" );
            var renderAction = new HtmlBlockRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "a", "b", string.Empty };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
