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
    public class AttributeValueSelectClauseTests
    {
        [Test]
        public void Select_NodeDoesNotBelongToPage_Throws()
        {
            var nodes = new[] { new PageAttribute( "page", "type", "v" ) };
            var clause = new AttributeValueSelectClause();

            Assert.Throws<InvalidOperationException>( () => clause.Select( nodes ) );
        }

        [Test]
        public void Select_NodeIsNoAttribute_ReturnsNull()
        {
            var body = new PageBody( PageName.Create( "a" ) );
            body.Consume( new PlainText() );
            var clause = new AttributeValueSelectClause();

            var match = clause.Select( body.Children ).SingleOrDefault();

            Assert.That( match, Is.Null );
        }

        [Test]
        public void Select_NodeIsAttribute_AttributeValueIsSelected()
        {
            var attr = new PageAttribute( "page", "type", "note" );
            var body = new PageBody( PageName.Create( "a" ), attr );
            var clause = new AttributeValueSelectClause();

            var match = clause.Select( body.Children ).Single();

            Assert.That( match.DisplayText, Is.InstanceOf<Link>() );
            Assert.That( ( (Link)match.DisplayText ).Text, Is.SameAs( attr.Value ) );
        }

        [Test]
        public void Select_MultipleAttributes_MultipleMatchCreated()
        {
            var attr1 = new PageAttribute( "gtd", "asap", "1" );
            var attr2 = new PageAttribute( "gtd", "asap", "2" );
            var body = new PageBody( PageName.Create( "a" ), attr1, attr2 );
            var clause = new AttributeValueSelectClause();

            var matches = clause.Select( body.Children );

            var selectedAttrValues = matches.Select( m => (Link)m.DisplayText )
                .Select( link => link.Text )
                .ToList();
            var expectedSelection = new[] { "1", "2" };
            Assert.That( selectedAttrValues, Is.EquivalentTo( expectedSelection ) );
        }
    }
}
