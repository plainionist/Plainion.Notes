using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class DynamicLinqWhereClauseTests 
    {
        [Test]
        public void Where_ExpressionMatchesNode_ReturnsTrue()
        {
            var query = MockFactory.CreateCompiledQuery( "page.type == \"node\"" );
            var clause = new DynamicLinqWhereClause( query.Definition.WhereExpression );

            var iterator = clause.CreateIterator( query );
            iterator.CurrentNode = new PageAttribute( "page", "type", "node" );
            var matches = clause.Where( iterator );

            Assert.IsTrue( matches );
        }

        [Test]
        public void Where_ExpressionNotMatchesNode_ReturnsFalse()
        {
            var query = MockFactory.CreateCompiledQuery( "page.type == \"node\"" );
            var clause = new DynamicLinqWhereClause( query.Definition.WhereExpression );

            var iterator = clause.CreateIterator( query );
            iterator.CurrentNode = new PageAttribute( "page", "type", "something else" );
            var matches = clause.Where( iterator );

            Assert.IsFalse( matches );
        }
    }
}
