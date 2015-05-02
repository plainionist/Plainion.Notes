using System.Linq;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class QueryExecutionStepTests 
    {
        [Test]
        public void Transform_NoCompiledQuery_AstRemainsUntouched()
        {
            var executeStep = new QueryExecutionStep();
            var page = new PageBody( PageName.Create( "a" ),
                new PlainText( "1" ), new PlainText( "2" ) );

            int beforeHashCode = page.GetDeepHashCode();
            executeStep.Transform( page, new EngineContext() );

            // if hashcode is the same the AST has not been modified
            Assert.That( page.GetDeepHashCode(), Is.EqualTo( beforeHashCode ) );
        }

        [Test]
        public void Transform_WithCompiledQuery_CompiledQueryReplacedByResults()
        {
            var executeStep = new QueryExecutionStep();
            var query = FakeFactory2.CreateCompiledQuery( node => true, new PageSelectClause() );
            var page = new PageBody( PageName.Create( "a" ), query );

            var ctx = new EngineContext();
            ctx.Query = new QueryEngine( FakeFactory2.CreateRepository( "p1", "p2" ) );
            executeStep.Transform( page, ctx );

            Assert.That( page.Children.Single(), Is.InstanceOf<Content>() );
        }
    }
}
