using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageTests
    {
        [Test]
        public void SetContent_WhenCalled_ContentIsSet()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.Content = body;

            Assert.That( page.Content, Is.SameAs( body ) );
        }

        [Test]
        public void SetContent_WhenCalled_ContentBecomesChild()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.Content = body;

            XAssert.IsChildOf( body, page );
        }

        [Test]
        public void SetContent_ContentAlreadySet_ContentIsReplaced()
        {
            var page = CreatePage();
            var oldBody = new PageBody();
            var newBody = new PageBody();
            page.Content = oldBody;

            page.Content = newBody;

            Assert.That( page.Content, Is.SameAs( newBody ) );
            XAssert.IsChildOf( newBody, page );
            XAssert.IsNoLongerChildOf( oldBody, page );
        }

        [Test]
        public void SetHeader_WhenCalled_HeaderIsSet()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.Header = body;

            Assert.That( page.Header, Is.SameAs( body ) );
        }

        [Test]
        public void SetHeader_WhenCalled_HeaderBecomesChild()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.Header = body;

            XAssert.IsChildOf( body, page );
        }

        [Test]
        public void SetHeader_HeaderAlreadySet_HeaderIsReplaced()
        {
            var page = CreatePage();
            var oldBody = new PageBody();
            var newBody = new PageBody();
            page.Header = oldBody;

            page.Header = newBody;

            Assert.That( page.Header, Is.SameAs( newBody ) );
            XAssert.IsChildOf( newBody, page );
            XAssert.IsNoLongerChildOf( oldBody, page );
        }

        [Test]
        public void SetFooter_WhenCalled_FooterIsSet()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.Footer = body;

            Assert.That( page.Footer, Is.SameAs( body ) );
        }

        [Test]
        public void SetFooter_WhenCalled_FooterBecomesChild()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.Footer = body;

            XAssert.IsChildOf( body, page );
        }

        [Test]
        public void SetFooter_FooterAlreadySet_FooterIsReplaced()
        {
            var page = CreatePage();
            var oldBody = new PageBody();
            var newBody = new PageBody();
            page.Footer = oldBody;

            page.Footer = newBody;

            Assert.That( page.Footer, Is.SameAs( newBody ) );
            XAssert.IsChildOf( newBody, page );
            XAssert.IsNoLongerChildOf( oldBody, page );
        }

        [Test]
        public void SetSideBar_WhenCalled_SideBarIsSet()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.SideBar = body;

            Assert.That( page.SideBar, Is.SameAs( body ) );
        }

        [Test]
        public void SetSideBar_WhenCalled_SideBarBecomesChild()
        {
            var page = CreatePage();
            var body = new PageBody();

            page.SideBar = body;

            XAssert.IsChildOf( body, page );
        }

        [Test]
        public void SetSideBar_SideBarAlreadySet_SideBarIsReplaced()
        {
            var page = CreatePage();
            var oldBody = new PageBody();
            var newBody = new PageBody();
            page.SideBar = oldBody;

            page.SideBar = newBody;

            Assert.That( page.SideBar, Is.SameAs( newBody ) );
            XAssert.IsChildOf( newBody, page );
            XAssert.IsNoLongerChildOf( oldBody, page );
        }

        private Page CreatePage()
        {
            return new Page( PageName.Create( "a" ) );
        }
    }
}
