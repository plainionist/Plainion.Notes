using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class QueryEngineTests
    {
        private class AllPagesMatcher : IQueryMatcher
        {
            public AllPagesMatcher()
            {
                MatchedPages = new List<PageName>();
            }

            public IList<PageName> MatchedPages
            {
                get;
                private set;
            }

            public IEnumerable<QueryMatch> Match( PageHandle page )
            {
                MatchedPages.Add( page.Name );
                return QueryMatch.Bundle( QueryMatch.CreatePageMatch( page.Name ) );
            }
        }

        [Test]
        public void Query_WhenCalled_MatcherIsAppliedToAllPages()
        {
            var repository = FakeFactory2.CreateRepository( "123", "abc", "xyz" );
            var engine = new QueryEngine( repository );

            var matcher = new AllPagesMatcher();
            var matches = engine.Where( matcher );

            var expectedMatchedPages = repository.Pages.Select( page => page.Name );
            Assert.That( matcher.MatchedPages, Is.EquivalentTo( expectedMatchedPages ) );
        }
    }
}
