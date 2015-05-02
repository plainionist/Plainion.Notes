using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using NUnit.Framework;
using Plainion.Wiki.UnitTests.Testing;
using Moq;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class DynamicQueryExecutorTests 
    {
        [Test]
        public void Execute_QueryNotAllowedOnPage_ReturnsEmptySet()
        {
            var page = new PageBody( PageName.Create( "a" ), new PlainText( "b" ) );
            var executor = CreateExecutor();
            Mock.Get( executor.Query.FromClause ).Setup( x => x.IsQueryFromPageAllowed( It.IsAny<PageBody>() ) ).Returns( false );

            var matches = executor.Execute( page );

            Assert.That( matches, Is.Empty );
        }

        [Test]
        public void Execute_WhereClauseMatches_ReturnsMatchingNodes()
        {
            var searchedNode = new PlainText( "c" );
            var page = new PageBody( PageName.Create( "a" ), new PlainText( "b" ), searchedNode );
            var executor = CreateExecutor( node => node == searchedNode );

            var matches = executor.Execute( page );

            var expectedNodes = new[] { searchedNode };
            var matchedNodes = matches.Select( match => match.DisplayText ).ToList();
            Assert.That( matchedNodes, Is.EquivalentTo( expectedNodes ) );
        }

        [Test]
        public void Execute_WhereClauseMatchesNot_ReturnsEmptySet()
        {
            var searchedNode = new PlainText( "not on that page" );
            var page = new PageBody( PageName.Create( "a" ), new PlainText( "b" ), new PlainText( "c" ) );
            var executor = CreateExecutor( node => node == searchedNode );

            var matches = executor.Execute( page );

            Assert.That( matches, Is.Empty );
        }

        private DynamicQueryExecutor CreateExecutor()
        {
            var query = MockFactory.CreateCompiledQuery( "true" );
            return new DynamicQueryExecutor( query );
        }

        private DynamicQueryExecutor CreateExecutor( Func<PageLeaf, bool> whereClause )
        {
            var query = FakeFactory2.CreateCompiledQuery( whereClause );
            var executor = new DynamicQueryExecutor( query );

            Mock.Get( query.FromClause ).Setup( x => x.IsQueryFromPageAllowed( It.IsAny<PageBody>() ) ).Returns( true );

            return executor;
        }
    }
}
