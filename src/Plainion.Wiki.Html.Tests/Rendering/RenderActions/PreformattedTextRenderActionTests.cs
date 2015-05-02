using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class PreformattedTextRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlPreBlockWritten()
        {
            var para = new PreformattedText();
            para.Consume(  "a"  );
            var renderAction = new PreformattedTextRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<pre>", "a", "</pre>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
