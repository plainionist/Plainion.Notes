using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class SiteSearchFormRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_SiteSearchFormWritten()
        {
            var para = new SiteSearchForm();
            var renderAction = new SiteSearchFormRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            Assert.That( output.First(), Contains.Substring( "<form method" ) );
            Assert.That( output.Last(), Contains.Substring( "</form>" ) );
        }
    }
}
