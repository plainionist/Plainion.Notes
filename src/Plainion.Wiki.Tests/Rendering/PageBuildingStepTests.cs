using NUnit.Framework;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class PageBuildingStepTests
    {
        [Test]
        public void Transform_WhenCalled_PageIsBuildUp()
        {
            var body = new PageBody(PageName.Create("a"));
            var step = new PageBuildingStep(new PageLayoutDescriptor());

            var ctx = new EngineContext();
            ctx.GetPage = GetPage;
            var pageNode = step.Transform(body, ctx);

            Assert.That(pageNode, Is.InstanceOf<Page>());
            var page = (Page)pageNode;
            Assert.That(page.Header, Is.Not.Null);
            Assert.That(page.Footer, Is.Not.Null);
            Assert.That(page.SideBar, Is.Not.Null);
            Assert.That(page.Content, Is.SameAs(body));
        }

        private PageBody GetPage(PageName pageName)
        {
            return new PageBody(pageName);
        }
    }
}
