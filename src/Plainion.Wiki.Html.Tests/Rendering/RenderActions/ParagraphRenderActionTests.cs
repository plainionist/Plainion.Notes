using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class ParagraphRenderActionTests : TestBase
    {
        [Test]
        public void Render_WithText_HtmlParagraphWritten()
        {
            var para = new Paragraph();
            para.Consume( new TextBlock( "a" ) );
            para.Consume( new TextBlock( "b" ) );
            var renderAction = new ParagraphRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<p>", "</p>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }

        [Test]
        public void Render_WithText_RenderingCalledForChildren()
        {
            var para = new Paragraph( new TextBlock( "a" ) );
            var renderAction = new ParagraphRenderAction();
            OnNestedRenderCall = node => RenderingContext.Writer.WriteLine( "@@@" );

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<p>", "@@@", "</p>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
