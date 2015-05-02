using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class BulletListRenderActionTests : TestBase
    {
        [Test]
        public void Render_NonOrderList_HtmlListWritten()
        {
            var list = new BulletList();
            var renderAction = new BulletListRenderAction();

            Render( renderAction, list );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<ul>", "</ul>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }

        [Test]
        public void Render_OrderList_HtmlListWritten()
        {
            var list = new BulletList();
            list.Ordered = true;
            var renderAction = new BulletListRenderAction();

            Render( renderAction, list );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<ol>", "</ol>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }

        [Test]
        public void Render_WithListItems_HtmlListWritten()
        {
            var list = new BulletList( new ListItem() );
            var renderAction = new BulletListRenderAction();
            OnNestedRenderCall = node => RenderingContext.Writer.WriteLine( "@@@" );

            Render( renderAction, list );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "<ul>", "@@@", "</ul>" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
