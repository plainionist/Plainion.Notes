using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class LinkReformattingStepTests : TestBase
    {
        [Test]
        public void Transform_ExternalUrl_HtmlLinkWritten()
        {
            var link = new Link( "http://www.google.de", "Google" );
            var content = new Content( link );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( link.Url ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( link.Text ) );
        }

        [Test]
        public void Transform_UrlWithoutText_HtmlLinkTextBecomesUrl()
        {
            var link = new Link( "http://www.google.de" );
            var content = new Content( link );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( link.Url ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( link.Url ) );
        }

        [Test]
        public void Transform_StaticUrl_HtmlLinkWritten()
        {
            var link = new Link( "a", "b" );
            link.IsStatic = true;
            var content = new Content( link );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( link.Url ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( link.Text ) );
        }

        [Test]
        public void Transform_NonExistingPage_LinkWithNewActionWritten()
        {
            var link = new Link( "TestPage", "Some page" );
            myContext.EngineContext.FindPageByName = ( ns, name ) => null;
            var content = new Content( link );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Content>( content );
            var text = XAssert.HasSingleChildOf<PlainText>( reformattedLink );
            var outputLink = XAssert.HasSingleChildOf<Link>( reformattedLink );
            Assert.That( text.Text, Is.EquivalentTo( "Some page" ) );
            Assert.That( outputLink.Url, Is.EquivalentTo( "TestPage?action=new" ) );
            Assert.That( outputLink.Text, Is.EquivalentTo( "?" ) );
        }

        [Test]
        public void Transform_ExistingPage_HtmlLinkWritten()
        {
            var link = new Link( "TestPage", "Some page" );
            myContext.EngineContext.FindPageByName = ( ns, name ) => PageName.CreateFromPath( "/TestPage" );
            var content = new Content( link );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( "/TestPage" ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( link.Text ) );
        }

        [Test]
        public void Transform_RelativePageName_HtmlLinkWritten()
        {
            var link = new Link( "c", "x" );
            var content = new PageBody( PageName.CreateFromPath( "/a/b" ), link );
            myContext.EngineContext.FindPageByName = ( ns, name ) => PageName.CreateFromPath( "/a/c" );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( "/a/c" ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( "x" ) );
        }

        [Test]
        public void Transform_RelativeAndAbsolutePageExists_PreferRelativePageOverAbsolute()
        {
            var link = new Link( "c", "x" );
            var content = new PageBody( PageName.CreateFromPath( "/a/b" ), link );
            // "always exists" => page exists in relative NS and absolute one
            myContext.EngineContext.FindPageByName = ( ns, name ) => PageName.CreateFromPath( "/a/c" );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( "/a/c" ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( "x" ) );
        }

        [Test]
        public void Transform_PageWithNamespaceButOnlyAbsolutePageExists_FallBackToAbsolutePage()
        {
            var link = new Link( "c", "x" );
            var content = new PageBody( PageName.CreateFromPath( "/a/b" ), link );
            myContext.EngineContext.FindPageByName = ( ns, name ) => PageName.CreateFromPath( "/c" );

            Transform( content );

            var reformattedLink = XAssert.HasSingleChildOf<Link>( content );
            Assert.That( reformattedLink.Url, Is.EquivalentTo( "/c" ) );
            Assert.That( reformattedLink.Text, Is.EquivalentTo( "x" ) );
        }

        private void Transform( PageLeaf root )
        {
            var step = new LinkReformattingStep();
            step.Transform( root, myContext.EngineContext );
        }
    }
}
