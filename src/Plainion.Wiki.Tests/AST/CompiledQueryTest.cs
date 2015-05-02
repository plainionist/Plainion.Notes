using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using Moq;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class CompiledQueryTest 
    {
        private QueryDefinition myDefinition;
        private IWhereClause myWhereClause;
        private ISelectClause mySelectClause;
        private IFromClause myFromClause;

        [SetUp]
        public void SetUp()
        {
            myDefinition = new QueryDefinition( "a" );
            myWhereClause = new Mock<IWhereClause> { DefaultValue = DefaultValue.Mock }.Object;
            mySelectClause = new Mock<ISelectClause>{ DefaultValue = DefaultValue.Mock }.Object;
            myFromClause = new Mock<IFromClause>{ DefaultValue = DefaultValue.Mock }.Object;
        }

        [Test]
        public void Ctor_WhenCalled_AllPropertiesShouldBeSet()
        {
            var query = new CompiledQuery( myDefinition, myWhereClause, mySelectClause, myFromClause );

            Assert.That( query.Definition, Is.SameAs( myDefinition ) );
            Assert.That( query.WhereClause, Is.SameAs( myWhereClause ) );
            Assert.That( query.SelectClause, Is.SameAs( mySelectClause ) );
            Assert.That( query.FromClause, Is.SameAs( myFromClause ) );
        }

        [Test]
        public void Ctor_DefinitionIsNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => new CompiledQuery( null, myWhereClause, mySelectClause, myFromClause ) );
        }

        [Test]
        public void Ctor_WhereClauseIsNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => new CompiledQuery( myDefinition, null, mySelectClause, myFromClause ) );
        }

        [Test]
        public void Ctor_SelectClauseIsNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => new CompiledQuery( myDefinition, myWhereClause, null, myFromClause ) );
        }

        [Test]
        public void Ctor_FromClauseIsNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>( () => new CompiledQuery( myDefinition, myWhereClause, mySelectClause, null ) );
        }
    }
}
