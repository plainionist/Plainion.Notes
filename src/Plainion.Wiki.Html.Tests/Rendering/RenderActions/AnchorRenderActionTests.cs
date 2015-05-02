using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class AnchorRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_AnchorIsWritten()
        {
            var text = new Anchor( "x" );
            var renderAction = new AnchorRenderAction();

            Render( renderAction, text );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<a name=\"x\"></a>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
