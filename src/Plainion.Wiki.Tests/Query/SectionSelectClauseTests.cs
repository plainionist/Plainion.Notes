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
    public class SectionSelectClauseTests
    {
        [Test]
        public void Select_NodeDoesNotBelongToPage_Throws()
        {
            var nodes = new[] { new PlainText() };
            var clause = new SectionSelectClause();

            Assert.Throws<InvalidOperationException>( () => clause.Select( nodes ) );
        }

        [Test]
        public void Select_NodeWithoutSection_CreatesPageMatch()
        {
            var body = new PageBody( PageName.Create( "a" ) );
            body.Consume( new PlainText() );
            var clause = new SectionSelectClause();

            var match = clause.Select( body.Children ).Single();

            XAssert.IsPageMatchOfPage( match, body.Name );
        }

        [Test]
        public void Select_NodeWithSection_SectionIsSelected()
        {
            var headline = new Headline( "a", 1 );
            var node = new PlainText();
            var body = new PageBody( PageName.Create( "a" ), headline, node );
            var clause = new SectionSelectClause();

            var match = clause.Select( new[] { node } ).Single();

            Assert.That( match.DisplayText, Is.InstanceOf<Link>() );
            Assert.That( ( (Link)match.DisplayText ).Text, Is.SameAs( headline.Text ) );
        }

        [Test]
        public void Select_MultipleNodesWithSameSection_OnlyOneMatchCreated()
        {
            var headline = new Headline( "a", 1 );
            var node1 = new PlainText();
            var node2 = new PlainText();
            var body = new PageBody( PageName.Create( "a" ), headline, node1, node2 );
            var clause = new SectionSelectClause();

            var matches = clause.Select( new[] { node1, node2 } );

            Assert.That( matches.Count(), Is.EqualTo( 1 ) );
        }

        [Test]
        public void Select_MultipleNodesWithDifferentSections_MultipleMatchsCreated()
        {
            var headline1 = new Headline( "a", 1 );
            var node1 = new PlainText();
            var node2 = new PlainText();
            var headline2 = new Headline( "b", 1 );
            var node3 = new PlainText();
            var node4 = new PlainText();
            var body = new PageBody( PageName.Create( "a" ), headline1, node1, node2, headline2, node3, node4 );
            var clause = new SectionSelectClause();

            var matches = clause.Select( new[] { node1, node2, node3, node4 } );

            var selectedSectionTexts = matches.Select( m => (Link)m.DisplayText )
                .Select( link => link.Text )
                .ToList();
            var expectedSelection = new[] { "a", "b" };
            Assert.That( selectedSectionTexts, Is.EquivalentTo( expectedSelection ) );
        }
    }
}
