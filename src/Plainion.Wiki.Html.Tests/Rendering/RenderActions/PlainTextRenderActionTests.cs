using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class PlainTextRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_TextIsWritten()
        {
            var text = new PlainText( "a" );
            var renderAction = new PlainTextRenderAction();

            Render( renderAction, text );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "a" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }

        [Test]
        public void Render_WhenCalled_EncodingIsHandledPropperly()
        {
            var text = new PlainText( "äöüß" );
            var renderAction = new PlainTextRenderAction();

            Render( renderAction, text );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "äöüß" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
