using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Query;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class PageSelectClauseTests
    {
        [Test]
        public void Select_NodeWithoutPageBody_Throws()
        {
            var clause = new PageSelectClause();
            var nodes = new List<PageLeaf>() { new PlainText() };

            Assert.Throws<InvalidOperationException>( () => clause.Select( nodes ) );
        }

        [Test]
        public void Select_NodeWithPageBody_PageMatchCreated()
        {
            var body = new PageBody( PageName.Create( "a" ) );
            body.Consume( new PlainText() );
            var clause = new PageSelectClause();

            var match = clause.Select( body.Children ).Single();

            XAssert.IsPageMatchOfPage( match, body.Name );
        }

        [Test]
        public void Select_MultipleNodes_OnlyOnePageMatchCreated()
        {
            var body = new PageBody( PageName.Create( "a" ) );
            body.Consume( new PlainText() );
            body.Consume( new PlainText() );
            var clause = new PageSelectClause();

            var matches = clause.Select( body.Children );

            Assert.That( matches.Count(), Is.EqualTo( 1 ) );
        }
    }
}
