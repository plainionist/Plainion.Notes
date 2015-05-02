using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class GenericRenderActionTests
    {
        private class TestableGenericRenderAction<TNode> : GenericRenderAction<TNode>
              where TNode : PageLeaf
        {
            public IRenderActionContext RenderActionContext
            {
                get { return Context; }
            }

            protected override void Render( TNode node )
            {
                // nothing to do
            }
        }

        /// <summary>
        /// See code comment inside the GenericRenderAction for reasons.
        /// </summary>
        [Test]
        public void RenderWithContext_WhenMethodLeft_ContextPropertyMustNotBeResetted()
        {
            var renderAction = new TestableGenericRenderAction<Content>();
            var ctx = new Mock<IRenderActionContext> { DefaultValue = DefaultValue.Mock }.Object;

            renderAction.Render( new Content(), ctx );

            Assert.That( renderAction.RenderActionContext, Is.SameAs( ctx ) );
        }

        [Test]
        public void Render_CalledWithNodeOfWrongType_Throws()
        {
            var renderAction = new TestableGenericRenderAction<PlainText>();
            var ctx = new Mock<IRenderActionContext> { DefaultValue = DefaultValue.Mock }.Object;

            Assert.Throws<ArgumentException>( () => renderAction.Render( new Content(), ctx ) );
        }
    }
}
