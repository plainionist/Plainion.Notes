using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using Plainion.Testing;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class QueryCompilationStepTests
    {
        [Test]
        public void Transform_NoQueryDefinition_AstRemainsUntouched()
        {
            var compileStep = CreateQueryCompilationStep();
            var page = new PageBody( PageName.Create( "a" ),
                new PlainText( "1" ), new PlainText( "2" ) );

            int beforeHashCode = page.GetDeepHashCode();
            compileStep.Transform( page, new EngineContext() );

            // if hashcode is the same the AST has not been modified
            Assert.That( page.GetDeepHashCode(), Is.EqualTo( beforeHashCode ) );
        }

        [Test]
        public void Transform_WithQueryDefinition_QueryDefinitionReplacedByCompiledQuery()
        {
            var compileStep = CreateQueryCompilationStep();
            var untouchedChild = new Link( PageName.Create( "b" ) );
            var page = new PageBody( PageName.Create( "a" ),
                new QueryDefinition( "true" ), untouchedChild );

            compileStep.Transform( page, new EngineContext() );

            Assert.That( page.Children.Count(), Is.EqualTo( 2 ) );
            Assert.That( page.Children.First(), Is.InstanceOf<CompiledQuery>() );
            Assert.That( page.Children.Last(), Is.SameAs( untouchedChild ) );
        }

        private QueryCompilationStep CreateQueryCompilationStep()
        {
            return new QueryCompilationStep( new FakeQueryCompiler(  ) );
        }
    }
}
