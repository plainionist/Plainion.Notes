using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class LambdaRenderActionTests 
    {
        [Test]
        public void Render_CalledWithNodeOfWrongType_Throws()
        {
            var renderAction = new LambdaRenderAction<PlainText>( ( node, ctx ) => { } );
            var context = new Mock<IRenderActionContext> { DefaultValue = DefaultValue.Mock }.Object;

            Assert.Throws<ArgumentException>( () => renderAction.Render( new Content(), context ) );
        }

        [Test]
        public void Render_WhenCalled_LambdaGetsCalled()
        {
            bool gotCalled = false;
            var renderAction = new LambdaRenderAction<PlainText>( ( node, ctx ) => gotCalled = true );
            var context = new Mock<IRenderActionContext> { DefaultValue = DefaultValue.Mock }.Object;

            renderAction.Render( new PlainText(), context );

            Assert.That( gotCalled, Is.True );
        }
    }
}
