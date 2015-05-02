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
    public class AstFinderTests
    {
        [Test]
        public void FirstOrDefault_NoNodeExists_ReturnsThisNode()
        {
            var root = new Content( new Link( "a" ) );
            var finder = new AstFinder<PlainText>( text => text.Text == "a" );

            var foundNode = finder.FirstOrDefault( root );

            Assert.That( foundNode, Is.Null );
        }

        [Test]
        public void FirstOrDefault_OneNodeExists_ReturnsThisNode()
        {
            var root = new Content( new PlainText( "a" ), new PlainText( "b" ) );
            var finder = new AstFinder<PlainText>( text => text.Text == "b" );

            var foundNode = finder.FirstOrDefault( root );

            Assert.That( foundNode, Is.SameAs( root.Children.Last() ) );
        }

        [Test]
        public void FirstOrDefault_TwoNodeExists_ReturnsTheFirst()
        {
            var root = new Content( new PlainText( "a" ), new PlainText( "a" ) );
            var finder = new AstFinder<PlainText>( text => text.Text == "a" );

            var foundNode = finder.FirstOrDefault( root );

            Assert.That( foundNode, Is.SameAs( root.Children.First() ) );
        }

        [Test]
        public void FirstOrDefault_RootIsTheSearchedNode_ReturnsRoot()
        {
            var root = new Content( new PlainText( "a" ), new PlainText( "a" ) );
            var finder = new AstFinder<Content>( x => true );

            var foundNode = finder.FirstOrDefault( root );

            Assert.That( foundNode, Is.SameAs( root ) );
        }

        [Test]
        public void Where_NoNodeExists_ReturnsThisNode()
        {
            var root = new Content( new Link( "a" ) );
            var finder = new AstFinder<PlainText>( text => text.Text == "a" );

            var foundNodes = finder.Where( root );

            Assert.That( foundNodes, Is.Empty );
        }

        [Test]
        public void Where_OneNodeExists_ReturnsThisNode()
        {
            var root = new Content( new PlainText( "a" ), new PlainText( "b" ) );
            var finder = new AstFinder<PlainText>( text => text.Text == "b" );

            var foundNodes = finder.Where( root );

            var expectedNodes = new[] { root.Children.Last() };
            Assert.That( foundNodes, Is.EquivalentTo( expectedNodes ) );
        }

        [Test]
        public void Where_TwoNodeExists_ReturnsTheFirst()
        {
            var text1 = new PlainText( "a" );
            var text2 = new PlainText( "a" );
            var root = new Content( text1, new Content( text2 ), new Link( "a" ) );
            var finder = new AstFinder<PlainText>( text => text.Text == "a" );

            var foundNodes = finder.Where( root );

            var expectedNodes = new[] { text1, text2 };
            Assert.That( foundNodes, Is.EquivalentTo( expectedNodes ) );
        }
    }
}
