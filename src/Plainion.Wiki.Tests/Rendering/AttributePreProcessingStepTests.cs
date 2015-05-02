using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.Utils;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class AttributePreProcessingStepTests
    {
        [Test]
        public void RenderValueOnDefinitionIfConfigured()
        {
            var settings = new AttributeRenderingSettings();
            settings.Attributes.Add( new AttributeRenderingStyle()
            {
                QualifiedName = "page.type",
                IsRenderValueOnDefinition = true,
                RenderValueOnDefinitionPrefix = "Page type: "
            } );

            var config = new SiteConfig();
            config.ComponentConfigs[ "AttributeRenderingSettings" ] = settings;

            var attrDefinition = new PageAttribute( "page", "type", "test" );

            var body = new PageBody();
            body.Consume( new Paragraph( attrDefinition ) );

            var transformedAst = Render( body, config );

            var finder = new AstFinder<PlainText>( text => text.Text == "test" );
            var result = finder.FirstOrDefault( transformedAst );

            Assert.That( result, Is.Not.Null );
        }

        [Test]
        public void RenderValueForDefinitionOnSamePage()
        {
            var attrDefinition = new PageAttribute( "page", "type", "test" );
            var attrReference = new PageAttribute( "page", "type" );

            var body = new PageBody();
            body.Consume( new Paragraph( attrDefinition ) );
            body.Consume( new Paragraph( attrReference ) );

            var transformedAst = Render( body, new SiteConfig() );

            var finder = new AstFinder<PlainText>( text => text.Text == "test" );
            var result = finder.FirstOrDefault( transformedAst );

            Assert.That( result, Is.Not.Null, "Attribute reference could not be resolved" );
        }

        private PageLeaf Render( PageLeaf node, SiteConfig config )
        {
            var ctx = new EngineContext();
            ctx.Config = config;
            ctx.PageExists = x => false;

            var step = new AttributePreProcessingStep();
            return step.Transform( node, ctx );
        }
    }
}
