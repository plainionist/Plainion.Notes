using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class ListItemRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_HtmlListItemWritten()
        {
            var para = new ListItem( "a" );
            var renderAction = new ListItemRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<li>", "</li>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }

        [Test]
        public void Render_WithText_RenderingCalledForChildren()
        {
            var para = new ListItem( "a" );
            var renderAction = new ListItemRenderAction();
            OnNestedRenderCall = node => RenderingContext.Writer.WriteLine( "@@@" );

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<li>", "@@@", "</li>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
