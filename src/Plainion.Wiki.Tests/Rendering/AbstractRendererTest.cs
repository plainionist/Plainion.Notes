using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Rendering;
using System.IO;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class AbstractRendererTest : TestBase
    {
        private class TestableRenderer : AbstractRenderer
        {
            public TestableRenderer()
            {
                RenderingActions = new Dictionary<Type, IRenderAction>();
            }

            public IDictionary<Type, IRenderAction> RenderingActions
            {
                get;
                set;
            }

            protected override IDictionary<Type, IRenderAction> RenderActions
            {
                get
                {
                    return RenderingActions;
                }
            }
        }

        private TestableRenderer myRenderer;

        [SetUp]
        public void SetUp()
        {
            myRenderer = new TestableRenderer();
        }

        [Test]
        public void RenderWithContext_WhenCalled_PreRenderingIsCalled()
        {
            bool preRenderingCalled = false;
            myRenderer.PreRendering += ( sender, eventArgs ) => preRenderingCalled = true;

            myRenderer.Render( new PageBody(), myContext );

            Assert.That( preRenderingCalled, Is.True );
        }

        [Test]
        public void RenderWithContext_WhenCalled_PostRenderingIsCalled()
        {
            bool postRenderingCalled = false;
            myRenderer.PostRendering += ( sender, eventArgs ) => postRenderingCalled = true;

            myRenderer.Render( new PageBody(), myContext );

            Assert.That( postRenderingCalled, Is.True );
        }

        [Test]
        public void RenderWithContext_WhenCalled_RenderingContextIsSetAndReset()
        {
            myRenderer.PreRendering += ( sender, eventArgs ) =>
                Assert.That( myRenderer.RenderingContext, Is.SameAs( myContext ) );

            myRenderer.Render( new PageBody(), myContext );

            Assert.That( myRenderer.RenderingContext, Is.Null );
        }

        [Test]
        public void RenderWithContext_WhenCalled_RootIsSetAndReset()
        {
            var root = new PageBody();

            myRenderer.PreRendering += ( sender, eventArgs ) =>
                Assert.That( myRenderer.Root, Is.SameAs( root ) );

            myRenderer.Render( root, myContext );

            Assert.That( myRenderer.Root, Is.Null );
        }

        [Test]
        public void Render_OutsideRenderingProcess_Throws()
        {
            Assert.Throws<InvalidOperationException>( () => myRenderer.Render( new PageBody() ) );
        }

        [Test]
        public void Render_NoRenderActionForNode_NothingGetsRendered()
        {
            var root = new Content();

            myRenderer.Render( root, myContext );

            var renderingOutput = GetRenderingOutput();
            Assert.That( renderingOutput, Is.Empty );
        }

        [Test]
        public void Render_RenderActionOnlyForNodeBaseClass_RenderActionOfBaseClassCalled()
        {
            var root = new EmptyText();
            myRenderer.RenderingActions[ typeof( PlainText ) ] = CreateEchoRenderAction( "CALLED" );

            myRenderer.Render( root, myContext );

            var renderingOutput = GetRenderingOutput();
            Assert.That( renderingOutput.Single(), Is.EqualTo( "CALLED" ) );
        }

        [Test]
        public void Render_RenderActionForNodeAndBaseClass_RenderActionOfNodeCalled()
        {
            var root = new EmptyText();
            myRenderer.RenderingActions[ typeof( EmptyText ) ] = CreateEchoRenderAction( "EmptyText" );
            myRenderer.RenderingActions[ typeof( PlainText ) ] = CreateEchoRenderAction( "PlainText" );

            myRenderer.Render( root, myContext );

            var renderingOutput = GetRenderingOutput();
            Assert.That( renderingOutput.Single(), Is.EqualTo( "EmptyText" ) );
        }

        private class EmptyText : PlainText
        {
        }

        private IRenderAction CreateEchoRenderAction( string message )
        {
            return new LambdaRenderAction<PlainText>(
                ( node, ctx ) => ctx.RenderingContext.Writer.WriteLine( message ) );
        }
    }
}
