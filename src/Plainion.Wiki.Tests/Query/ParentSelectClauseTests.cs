using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Query;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class ParentSelectClauseTests
    {
        [Test]
        public void Select_NodeWithoutParent_Throws()
        {
            var clause = new ParentSelectClause();
            var nodes = new List<PageLeaf>() { new PlainText() };

            Assert.Throws<InvalidOperationException>( () => clause.Select( nodes ) );
        }

        [Test]
        public void Select_NodeWithParent_ParentIsSelected()
        {
            var content = new Content( new PlainText() );
            var clause = new ParentSelectClause();

            var match = clause.Select( content.Children ).Single();

            Assert.That( match.DisplayText, Is.SameAs( content ) );
        }

        [Test]
        public void Select_MultipleNodesWithSameParent_OnlyOneMatchCreated()
        {
            var content = new Content( new PlainText(), new PlainText() );
            var clause = new ParentSelectClause();

            var matches = clause.Select( content.Children );

            Assert.That( matches.Count(), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Select_MultipleNodesWithDifferentParent_MultipleMatchsCreated()
        {
            var parent1 = new Content( new PlainText(), new PlainText() );
            var parent2 = new Content( new PlainText(), new PlainText() );
            var clause = new ParentSelectClause();

            var matches = clause.Select( parent1.Children.Concat( parent2.Children ) );

            var selectedParents = matches.Select( m => m.DisplayText ).ToList();
            var expectedSelection = new List<Content>() { parent1, parent2 };
            Assert.That( selectedParents, Is.EquivalentTo( expectedSelection ) );
        }
    }
}
