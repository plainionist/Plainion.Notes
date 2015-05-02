using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class AttributeTransformationStepTests
    {
        [Test]
        public void Transform_NoAttributeForRegisteredTransformers_NoTransformerCalled()
        {
            var catalog = new PageAttributeTransformerCatalog();
            var transformer = new Mock<IPageAttributeTransformer> { DefaultValue = DefaultValue.Mock };
            catalog.Plugins[ "x.y" ] = transformer.Object;

            var root = new PageBody( PageName.Create( "1" ) );
            root.Consume( new PageAttribute( "a", "b" ) );

            var transformerStep = new AttributeTransformationStep( catalog );
            transformerStep.Transform( root, new EngineContext() );

            transformer.Verify( x => x.Transform( It.IsAny<PageAttribute>(), It.IsAny<EngineContext>() ), Times.Never() );
        }

        [Test]
        public void Transform_WithAttributeForRegisteredTransformers_TransformerGetsCalled()
        {
            var catalog = new PageAttributeTransformerCatalog();
            var transformer = new Mock<IPageAttributeTransformer> { DefaultValue = DefaultValue.Mock };
            catalog.Plugins[ "a.b" ] = transformer.Object;

            var root = new PageBody( PageName.Create( "1" ) );
            root.Consume( new PageAttribute( "a", "b" ) );

            var transformerStep = new AttributeTransformationStep( catalog );
            transformerStep.Transform( root, new EngineContext() );

            transformer.Verify( x => x.Transform( It.IsAny<PageAttribute>(), It.IsAny<EngineContext>() ), Times.Once() );
        }
    }
}
