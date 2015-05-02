using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class ContentRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_NotOutputWritten()
        {
            var para = new Content();
            var renderAction = new ContentRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            Assert.That( output, Is.Empty );
        }

        [Test]
        public void Render_WithChildren_RenderingCalledForChildren()
        {
            var para = new Content( new PlainText( "a" ) );
            var renderAction = new ContentRenderAction();
            OnNestedRenderCall = node => RenderingContext.Writer.WriteLine( "@@@" );

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "@@@" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
