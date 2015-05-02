using System.IO;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class RenderingPipelineTests
    {
        [Test]
        public void Ctor_WhenCalled_RendererIsSet()
        {
            var catalog = new RenderingStepCatalog();
            var renderer = new Mock<IRenderer> { DefaultValue = DefaultValue.Mock }.Object;

            var pipeline = new RenderingPipeline( catalog, renderer );

            Assert.That( pipeline.Renderer, Is.SameAs( renderer ) );
        }

        [Test]
        public void Ctor_WhenCalled_StepsAreSortedByStage()
        {
            var renderer = new Mock<IRenderer> { DefaultValue = DefaultValue.Mock }.Object;
            var catalog = new RenderingStepCatalog();
            var step1 = new Mock<IRenderingStep> { DefaultValue = DefaultValue.Mock }.Object;
            var step2 = new Mock<IRenderingStep> { DefaultValue = DefaultValue.Mock }.Object;
            var step3 = new Mock<IRenderingStep> { DefaultValue = DefaultValue.Mock }.Object;
            catalog.Plugins.Add( 400, step1 );
            catalog.Plugins.Add( 100, step2 );
            catalog.Plugins.Add( 700, step3 );

            var pipeline = new RenderingPipeline( catalog, renderer );

            var expectedSteps = new[] { step2, step1, step3 };
            Assert.That( pipeline.Steps, Is.EquivalentTo( expectedSteps ) );
        }

        [Test]
        public void Render_WhenCalled_StepsAreCalled()
        {
            var renderer = new Mock<IRenderer> { DefaultValue = DefaultValue.Mock }.Object;
            var catalog = new RenderingStepCatalog();
            var step1 = new Mock<IRenderingStep> { DefaultValue = DefaultValue.Mock };
            var step2 = new Mock<IRenderingStep> { DefaultValue = DefaultValue.Mock };
            catalog.Plugins.Add( 400, step1.Object );
            catalog.Plugins.Add( 100, step2.Object );
            var pipeline = new RenderingPipeline( catalog, renderer );

            var ctx = CreateContext();
            pipeline.Render( new PageBody(), ctx );

            step1.Verify( x => x.Transform( It.IsAny<PageLeaf>(), It.IsAny<EngineContext>() ), Times.Once() );
            step2.Verify( x => x.Transform( It.IsAny<PageLeaf>(), It.IsAny<EngineContext>() ), Times.Once() );
        }

        [Test]
        public void Render_WhenCalled_RendererIsCalled()
        {
            var renderer = new Mock<IRenderer> { DefaultValue = DefaultValue.Mock };
            var catalog = new RenderingStepCatalog();
            var pipeline = new RenderingPipeline( catalog, renderer.Object );

            var ctx = CreateContext();
            pipeline.Render( new PageBody(), ctx );

            renderer.Verify( x => x.Render( It.IsAny<PageLeaf>(), It.IsAny<RenderingContext>() ), Times.Once() );
        }

        private RenderingContext CreateContext()
        {
            var ctx = new RenderingContext( new MemoryStream() );
            ctx.EngineContext = new EngineContext();
            return ctx;
        }
    }
}
