using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.UnitTests.Utils
{
    [TestFixture]
    public class AstExtensionsTests
    {
        [Test]
        public void FindRelatedHeadline_NoRelatedHeadline_ReturnsNull()
        {
            var node = new PlainText();
            var paragraph = new Paragraph( node );

            var headline = node.FindRelatedHeadline();

            Assert.That( headline, Is.Null );
        }

        [Test]
        public void FindRelatedHeadline_HeadlineIsSibling_ReturnsThatHeadline()
        {
            var headline = new Headline( "a", 1 );
            var node = new PlainText();
            var paragraph = new Content( headline, node );

            var foundHeadline = node.FindRelatedHeadline();

            Assert.That( foundHeadline, Is.SameAs( headline ) );
        }

        [Test]
        public void FindRelatedHeadline_HeadlineIsSiblingOfParent_ReturnsThatHeadline()
        {
            var headline = new Headline( "a", 1 );
            var node = new PlainText();
            var paragraph = new Content( headline, new Content( node ) );

            var foundHeadline = node.FindRelatedHeadline();

            Assert.That( foundHeadline, Is.SameAs( headline ) );
        }

        [Test]
        public void IsLinkingPage_ExternalLink_ReturnsFalse()
        {
            var link = new Link( "http://google.de" );
            var page = PageName.Create( "a" );

            Assert.IsFalse( link.IsLinkingPage( page ) );
        }

        [Test]
        public void IsLinkingPage_RootedLinkToPage_ReturnsTrue()
        {
            var link = new Link( "/a" );
            var page = PageName.Create( "a" );

            Assert.IsTrue( link.IsLinkingPage( page ) );
        }

        [Test]
        public void IsLinkingPage_RootedLinkToOtherPage_ReturnsFalse()
        {
            var link = new Link( "/a" );
            var page = PageName.Create( "b" );

            Assert.IsFalse( link.IsLinkingPage( page ) );
        }

        [Test]
        public void IsLinkingPage_LinkToRootedPageName_ReturnsTrue()
        {
            var link = new Link( "a" );
            var page = PageName.Create( "a" );

            Assert.IsTrue( link.IsLinkingPage( page ) );
        }

        [Test]
        public void IsLinkingPage_LinkOutsiePageToNonRootedPageName_ReturnsFalse()
        {
            var link = new Link( "a" );
            var page = PageName.CreateFromPath( "/b/a" );

            Assert.IsFalse( link.IsLinkingPage( page ) );
        }

        [Test]
        public void IsLinkingPage_RelativeLinkToOtherPage_ReturnsFalse()
        {
            var link = new Link( "c" );
            var body = new PageBody( PageName.CreateFromPath( "/a/b" ), link );
            var page = PageName.CreateFromPath( "/a/b" );

            Assert.IsFalse( link.IsLinkingPage( page ) );
        }

        [Test]
        public void IsLinkingPage_RelativeLinkToPage_ReturnsTrue()
        {
            var link = new Link( "b" );
            var body = new PageBody( PageName.CreateFromPath( "/a/b" ), link );
            var page = PageName.CreateFromPath( "/a/b" );

            Assert.IsTrue( link.IsLinkingPage( page ) );
        }

        [Test]
        public void GetFlattenedTree_NodeWithoutChildren_JustReturnsThatNode()
        {
            var node = new Content();

            var flatTree = node.GetFlattenedTree();

            var expectedNodes = new[] { node };
            Assert.That( flatTree, Is.EquivalentTo( expectedNodes ) );
        }

        [Test]
        public void GetFlattenedTree_WithFlatTree_JustReturnsTheChildren()
        {
            var node = new Content( new PlainText(), new PlainText() );

            var flatTree = node.GetFlattenedTree();

            var expectedNodes = new[] { node }.Concat( node.Children );
            Assert.That( flatTree, Is.EquivalentTo( expectedNodes ) );
        }

        [Test]
        public void GetFlattenedTree_WithNestedTree_JustReturnsTheChildren()
        {
            var c1 = new Content();
            var n1 = new Content( c1 );
            var c2 = new Content();
            var n2 = new Content( c2 );

            var node = new Content( n1, n2 );

            var flatTree = node.GetFlattenedTree();

            var expectedNodes = new[] { node, n1, c1, n2, c2 };
            Assert.That( flatTree, Is.EquivalentTo( expectedNodes ) );
        }

        [Test]
        public void GetNameOfPage_RootIsPageBody_ReturnsNameOfPageBody()
        {
            var node = new PlainText();
            var pageName = PageName.Create( "a" );
            var root = new PageBody( pageName, node );

            var name = node.GetNameOfPage();

            Assert.That( name, Is.SameAs( pageName ) );
        }

        [Test]
        public void GetNameOfPage_RootIsPage_ReturnsNameOfPage()
        {
            var node = new PlainText();
            var content = new PageBody( PageName.Create( "b" ), node );
            var pageName = PageName.Create( "a" );
            var page = new Page( pageName );
            page.Content = content;

            var name = node.GetNameOfPage();

            Assert.That( name, Is.SameAs( pageName ) );
        }

        [Test]
        public void GetNameOfPage_RootIsNotPageOrPageBody_ReturnsNull()
        {
            var node = new PlainText();
            var root = new Content( node );

            var name = node.GetNameOfPage();

            Assert.That( name, Is.Null );
        }
    }
}
