using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class PageBodyRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlPageBodyWritten()
        {
            var para = new PageBody();
            var renderAction = new PageBodyRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<body>", "</body>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }

        [Test]
        public void Render_WithText_RenderingCalledForChildren()
        {
            var para = new PageBody( new PlainText( "a" ) );
            var renderAction = new PageBodyRenderAction();
            OnNestedRenderCall = node => RenderingContext.Writer.WriteLine( "@@@" );

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<body>", "@@@", "</body>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
