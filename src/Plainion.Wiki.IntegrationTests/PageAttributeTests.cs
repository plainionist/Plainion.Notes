using Plainion.Testing;
using Plainion.Wiki.IntegrationTests.Tasks;
using Plainion.Wiki.Resources;
using NUnit.Framework;

namespace Plainion.Wiki.IntegrationTests
{
    [TestFixture]
    public class PageAttributeTests : TestBase
    {
        [Test]
        public void RenderDefaultHeader()
        {
            var pageName = Engine.CreatePage( "Page1", DocumentRoot.GetResource( ResourceNames.PageHeader ) );

            var output = Engine.Render( pageName );

            var expected = LoadTestData( "PageAttributes_DefaultHeader_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void RenderPageName()
        {
            var pageName = Engine.CreatePage( "Page1", "[@page.name]" );

            var output = Engine.Render( pageName );

            var expected = LoadTestData( "PageAttributes_PageName_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void RenderReverseLinks()
        {
            var page1 = Engine.CreatePage( "Page1", "[@page.reverselinks]" );
            var page2 = Engine.CreatePage( "Page2", "[Page1]" );

            var output = Engine.Render( page1 );

            var expected = LoadTestData( "PageAttributes_ReverseLinks_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void RenderRecentEdits()
        {
            var page1 = Engine.CreatePage( "Page1" );
            var page2 = Engine.CreatePage( "Page2", "[@site.recentedits]" );

            var output = Engine.Render( page2 );

            var expected = LoadTestData( "PageAttributes_RecentEdits_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }
    }
}
