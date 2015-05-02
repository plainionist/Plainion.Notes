using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;
using System;
using Plainion.Wiki.UnitTests.Testing;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageNodeTest
    {
        private class ConsumesNothingNode : PageNode
        {
            public ConsumesNothingNode()
            {
            }
        }

        private class CanConsumeNode : PageNode
        {
            public CanConsumeNode( params PageLeaf[] children )
                : base( children )
            {
            }

            public bool DenyConsume
            {
                get;
                set;
            }

            protected override bool CanConsume( PageLeaf part )
            {
                return !DenyConsume;
            }

            protected override void ConsumeInternal( PageLeaf part )
            {
                AddChild( part );
            }

            public void RemoveAllChildren_()
            {
                this.RemoveAllChildren();
            }
        }

        [Test]
        public void Ctor_WhenCalled_ParentIsNull()
        {
            var node = new ConsumesNothingNode();

            Assert.IsNull( node.Parent );
        }

        [Test]
        public void Ctor_WithoutChildren_ChildrenAreEmpty()
        {
            var node = new ConsumesNothingNode();

            Assert.AreEqual( 0, node.Children.Count() );
        }

        [Test]
        public void Ctor_WithNull_Throws()
        {
            Assert.Throws<ArgumentNullException>( () => new CanConsumeNode( null ) );
        }

        [Test]
        public void Ctor_WithChildren_ChildrenAreAdded()
        {
            var child = new PlainText( "hiho" );

            var node = new CanConsumeNode( child );

            Assert.That( node.Children.Single(), Is.EqualTo( child ) );
        }

        [Test]
        public void Consume_WithAcceptedType_ParentOfChildWillBeSet()
        {
            var parent = new Content();
            var node = new CanConsumeNode();

            parent.Consume( node );

            Assert.That( node.Parent, Is.EqualTo( parent ) );
        }

        [Test]
        public void Consume_WithUnsupportedPart_WillBePassedToParent()
        {
            var parent = new Content();
            var node = new CanConsumeNode();
            node.DenyConsume = true;
            parent.Consume( node );

            node.Consume( new PlainText( "hiho" ) );

            Assert.AreEqual( 0, node.Children.Count() );
            Assert.AreEqual( 2, parent.Children.Count() );
            var text = XAssert.HasSingleChildOf<PlainText>( parent );
            Assert.AreEqual( "hiho", text.Text );
        }

        [Test]
        public void Consume_UnsupportedPartWithoutParent_Throws()
        {
            var node = new ConsumesNothingNode();

            Assert.Throws<InvalidOperationException>( () => node.Consume( new PlainText( "hiho" ) ) );
        }

        [Test]
        public void RemoveAllChildren_WhenCalled_ClearsChildrenAndResetsParentReference()
        {
            var parent = new CanConsumeNode();
            var child1 = new ConsumesNothingNode();
            var child2 = new ConsumesNothingNode();
            var child3 = new ConsumesNothingNode();

            parent.Consume( child1 );
            parent.Consume( child2 );
            parent.Consume( child3 );

            parent.RemoveAllChildren_();

            Assert.That( child1.Parent, Is.Null );
            Assert.That( child2.Parent, Is.Null );
            Assert.That( child3.Parent, Is.Null );
            Assert.That( parent.Children.Count(), Is.EqualTo( 0 ) );
        }

        [Test]
        public void ReplaceChild_WhenCalled_ChildWillBeReplacedAndParentReferenceChanged()
        {
            var parent = new CanConsumeNode();
            var oldChild = new ConsumesNothingNode();
            parent.Consume( oldChild );

            var newChild = new ConsumesNothingNode();
            parent.ReplaceChild( oldChild, newChild );

            Assert.That( oldChild.Parent, Is.Null );
            Assert.That( newChild.Parent, Is.SameAs( parent ) );
            Assert.That( parent.Children.Single(), Is.SameAs( newChild ) );
        }

        [Test]
        public void RemoveChild_ChildDoesNotExist_NothingHappens()
        {
            var content = new Content();
            var child = new LineBreak();

            content.RemoveChild( child );

            Assert.IsFalse( content.Children.Any() );
        }

        [Test]
        public void RemoveChild_ChildExists_ChildWillBeRemoved()
        {
            var child = new LineBreak();
            var content = new Content( child );

            content.RemoveChild( child );

            Assert.IsFalse( content.Children.Any() );
        }

        [Test]
        public void RemoveChild_ChildExists_ParentOfChildSetToNull()
        {
            var child = new LineBreak();
            var content = new Content( child );

            content.RemoveChild( child );

            Assert.That( child.Parent, Is.Null );
        }

        [Test]
        public void RemoveChild_WithNull_Throws()
        {
            var content = new Content();

            Assert.Throws<ArgumentNullException>( () => content.RemoveChild( null ) );
        }
    }
}
