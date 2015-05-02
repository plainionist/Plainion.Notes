using System.Linq;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.AST;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.AST
{
    [TestFixture]
    public class HtmlBlockTest
    {
        [Test]
        public void Ctor_WithoutLines_NoContentAdded()
        {
            var html = new HtmlBlock();

            Assert.That( html.Html, Is.Empty );
        }

        [Test]
        public void Ctor_WithLines_LinesAreAddedToContent()
        {
            var html = new HtmlBlock( "<p>", "a", "</p>" );

            var lines = html.Html.AsLines().ToList();
            var expectedLines = new[] { "<p>", "a", "</p>" };
            Assert.That( lines, Is.EquivalentTo( expectedLines ) );
        }

        [Test]
        public void AppendLine_WhenCalled_AddsLineToContent()
        {
            var html = new HtmlBlock();

            html.AppendLine( "<br/>" );

            var lines = html.Html.AsLines().ToList();
            var expectedLines = new[] { "<br/>" };
            Assert.That( lines, Is.EquivalentTo( expectedLines ) );
        }
    }
}
