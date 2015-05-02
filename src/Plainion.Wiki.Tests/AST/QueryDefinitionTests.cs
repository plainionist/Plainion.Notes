using System;
using Plainion.Wiki.AST;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class QueryDefinitionTests
    {
        [Test]
        public void Ctor_WhenCalled_AttributeTypeEqualsQuery()
        {
            var query = new QueryDefinition( "a" );

            Assert.That( query.Type, Is.EqualTo( "query" ) );
        }

        [Test]
        public void Ctor_WhenCalled_AttributeNameIsNull()
        {
            var query = new QueryDefinition( "a" );

            Assert.That( query.Name, Is.Null );
        }

        [Test]
        public void Ctor_WithNullWhereExpr_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => new QueryDefinition( null ) );
        }

        [Test]
        public void Ctor_WithEmptyWhereExpr_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => new QueryDefinition( string.Empty ) );
        }

        [Test]
        public void Ctor_WithAllExpressions_PropertiesShouldBeSet()
        {
            var query = new QueryDefinition( "a", "b", "c" );

            Assert.That( query.WhereExpression, Is.EqualTo( "a" ) );
            Assert.That( query.SelectExpression, Is.EqualTo( "b" ) );
            Assert.That( query.FromExpression, Is.EqualTo( "c" ) );
        }
    }
}
