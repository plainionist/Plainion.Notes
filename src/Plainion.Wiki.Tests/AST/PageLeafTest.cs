using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageLeafTest
    {
        private class DummyLeaf : PageLeaf
        {
            public DummyLeaf()
            {
            }
        }

        [Test]
        public void Ctor_WhenCalled_ParentAndRootAreNull()
        {
            var node = new DummyLeaf();

            Assert.IsNull( node.Parent );
            Assert.IsNull( node.GetRoot() );
        }

        [Test]
        public void GetParent_WithParent_ReturnsParent()
        {
            var root = new PageBody();
            var node = new Content();

            root.Consume( node );

            Assert.That( root, Is.SameAs( node.Parent ) );
        }

        [Test]
        public void GetRoot_WithRoot_ReturnsRoot()
        {
            var root = new PageBody();
            var parent = new Content();
            var node = new DummyLeaf();

            root.Consume( parent );
            parent.Consume( node );

            Assert.That( root, Is.SameAs( node.GetRoot() ) );
        }

        [Test]
        public void GetParentOfType_SuchParentAvailable_ReturnsParent()
        {
            var root = new PageBody();
            var parent = new Content();
            var node = new DummyLeaf();

            root.Consume( parent );
            parent.Consume( node );

            Assert.That( parent, Is.SameAs( node.GetParentOfType<Content>() ) );
            Assert.That( root, Is.SameAs( node.GetParentOfType<PageBody>() ) );
        }

        [Test]
        public void GetParentOfType_NoSuchParent_ReturnsNull()
        {
            var root = new PageBody();
            var parent = new Content();
            var node = new DummyLeaf();

            root.Consume( parent );
            parent.Consume( node );

            Assert.That( node.GetParentOfType<Anchor>(), Is.Null );
        }
    }
}
