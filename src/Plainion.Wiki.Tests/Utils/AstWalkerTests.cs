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
    public class AstWalkerTests
    {
        [Test]
        public void Visit_NoMatchingNode_ActionNotCalled()
        {
            var root = new Content( new PlainText() );
            bool called = false;
            var walker = new AstWalker<Link>( link => called = true );

            walker.Visit( root );

            Assert.That( called, Is.False );
        }

        [Test]
        public void Visit_WithMatchingNodes_ActionCalledForThoseNodesOnly()
        {
            var root = new Content( new PlainText(), new Link( "a" ) );
            bool called = false;
            var walker = new AstWalker<Link>( link => called = true );

            walker.Visit( root );

            Assert.That( called, Is.True );
        }

        [Test]
        public void Visit_WholeTree_ActionCalledForWholeTree()
        {
            var root = new Content( new PlainText(), new Link( "a" ) );
            var visitedNodes = new List<PageLeaf>();
            var walker = new AstWalker<PageLeaf>( n => visitedNodes.Add( n ) );

            walker.Visit( root );

            var expectedNodes = new[] { root }.Concat( root.Children );
            Assert.That( visitedNodes, Is.EquivalentTo( expectedNodes ) );
        }
    }
}
