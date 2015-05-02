using Plainion.Wiki.AST;
using NUnit.Framework;
using System;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class LinkTest
    {
        [Test]
        public void Ctor_UrlWithoutAnchor_UrlWillBeKeptAsIs()
        {
            var url = "http://www.mydomain.de";

            var link = new Link( url );

            Assert.That( link.Url, Is.EqualTo( url ) );
        }

        [Test]
        public void Ctor_UrlWithoutAnchor_UrlWithoutAnchorPropertyEqualsUrl()
        {
            var link = new Link( @"SandBox" );

            Assert.That( link.UrlWithoutAnchor, Is.EqualTo( link.Url ) );
        }

        [Test]
        public void Ctor_UrlWithoutText_TextShouldEqualUrl()
        {
            var link = new Link( "http://www.mydomain.de" );

            Assert.That( link.Text, Is.EqualTo( link.Url ) );
        }

        [Test]
        public void Ctor_UrlAndText_ShouldBeByPassed()
        {
            var link = new Link( "http://www.mydomain.de", "a" );

            Assert.That( link.Url, Is.EqualTo( "http://www.mydomain.de" ) );
            Assert.That( link.Text, Is.EqualTo( "a" ) );
        }

        [Test]
        public void Ctor_PageName_UrlEqualsPageFullName()
        {
            var pageName = PageName.CreateFromPath( "/g/p" );

            var link = new Link( pageName );

            Assert.That( link.Url, Is.EqualTo( pageName.FullName ) );
        }

        [Test]
        public void Ctor_PageName_TextEqualsNotRootedPageFullName()
        {
            var pageName = PageName.CreateFromPath( "/g/p" );
            var nonRootedFullName = pageName.FullName.Substring( 1 );

            var link = new Link( pageName );

            Assert.That( link.Text, Is.EqualTo( nonRootedFullName ) );
        }

        [Test]
        public void Ctor_HttpUrl_ShouldBeExternal()
        {
            var link = new Link( "http://www.mydomain.de" );

            Assert.AreEqual( "http://www.mydomain.de", link.Url );
            Assert.IsTrue( link.IsExternal );
        }

        [Test]
        public void Ctor_FileUrl_ShouldBeExternal()
        {
            var link = new Link( "file://www.mydomain.de" );

            Assert.IsTrue( link.IsExternal );
        }

        [Test]
        public void Ctor_FileUrlForFirefox_ShouldBeExternal()
        {
            var link = new Link( "file://///www.mydomain.de" );

            Assert.IsTrue( link.IsExternal );
        }

        [Test]
        public void Ctor_UncPath_ShouldBeExternal()
        {
            var link = new Link( @"\\server\share" );

            Assert.IsTrue( link.IsExternal );
        }

        [Test]
        public void Ctor_PageNameAsUrl_ShouldNotBeExternal()
        {
            var link = new Link( "SandBox" );

            Assert.IsFalse( link.IsExternal );
        }

        [Test]
        public void Ctor_PageNameAsUrl_UrlShouldEqualPageName()
        {
            var link = new Link( "SandBox" );

            Assert.That( link.Url, Is.EqualTo( "SandBox" ) );
        }

        [Test]
        public void Ctor_UrlWithTextAndAnchor_UrlShouldBeParsedProperly()
        {
            var link = new Link( "http://www.mydomain.de#myplace", "MyDomain" );

            Assert.AreEqual( "http://www.mydomain.de#myplace", link.Url );
            Assert.AreEqual( "http://www.mydomain.de", link.UrlWithoutAnchor );
            Assert.AreEqual( "MyDomain", link.Text );
            Assert.AreEqual( "myplace", link.Anchor );
        }

        [Test]
        public void Ctor_PageNameAndTextAndAnchor_PropertiesShouldBeSet()
        {
            var link = new Link( "FocusMoney2009Heft35", "Fisher", "Heft35" );

            Assert.AreEqual( "FocusMoney2009Heft35#Fisher", link.Url );
            Assert.AreEqual( "FocusMoney2009Heft35", link.UrlWithoutAnchor );
            Assert.AreEqual( "Heft35", link.Text );
            Assert.AreEqual( "Fisher", link.Anchor );
        }

        [Test]
        public void Ctor_WithEmptyUrl_Throws( [Values( null, "" )] string url )
        {
            Assert.Throws<ArgumentNullException>( () => new Link( url, "x" ) );
        }
    }
}
