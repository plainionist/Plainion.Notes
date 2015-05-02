using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class LineBreakRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlLineBreakJustWritten()
        {
            var para = new LineBreak();
            var renderAction = new LineBreakRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<br/>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
