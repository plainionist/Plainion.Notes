using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class QueryIteratorTests
    {
        [Test]
        public void GetIdentifierValue_CurrentNodeIsNoPageAttribute_ReturnsNull()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new Content();

            var value = iterator.GetIdentifierValue( "page.type" );

            Assert.That( value, Is.Null );
        }

        [Test]
        public void GetIdentifierValue_PageAttributeIsReference_ReturnsNull()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PageAttribute( "page", "type" );

            var value = iterator.GetIdentifierValue( "page.type" );

            Assert.That( value, Is.Null );
        }

        [Test]
        public void GetIdentifierValue_UnknownPageAttributeDefinition_ReturnsNull()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PageAttribute( "gtd", "asap", "true" );

            var value = iterator.GetIdentifierValue( "page.type" );

            Assert.That( value, Is.Null );
        }

        [Test]
        public void GetIdentifierValue_MatchingPageAttributeDefinition_ReturnsAttributeValue()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PageAttribute( "page", "type", "note" );

            var value = iterator.GetIdentifierValue( "page.type" );

            Assert.That( value, Is.EqualTo( "note" ) );
        }

        [Test]
        public void Linked_CurrentNodeHasNoBody_ReturnsFalse()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PageAttribute( "page", "type", "note" );

            bool value = iterator.Linked();

            Assert.IsFalse( value );
        }

        [Test]
        public void Linked_CurrentPageIsNotLinked_ReturnsFalse()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PlainText( "b" );
            var queryPage = new PageBody( PageName.Create( "a" ), iterator.Query );
            var currentPage = new PageBody( PageName.Create( "b" ), iterator.CurrentNode );

            bool value = iterator.Linked();

            Assert.IsFalse( value );
        }

        [Test]
        public void Linked_CurrentPageIsLinked_ReturnsTrue()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PlainText( "b" );
            var currentPage = new PageBody( PageName.Create( "b" ), iterator.CurrentNode );
            var queryPage = new PageBody( PageName.Create( "a" ), iterator.Query, new Link( currentPage.Name ) );

            bool value = iterator.Linked();

            Assert.IsTrue( value );
        }

        [Test]
        public void Defined_PageAttributeIsOnlyReferencedOnPage_ReturnsFalse()
        {
            var page = new PageBody( PageName.Create( "a" ), new PageAttribute( "page", "type" ) );
            var iterator = CreateIterator();
            iterator.CurrentNode = page;

            bool value = iterator.Defined( "page.type" );

            Assert.IsFalse( value );
        }

        [Test]
        public void Defined_PageAttributeNotExistsOnPage_ReturnsFalse()
        {
            var page = new PageBody( PageName.Create( "a" ), new PlainText( "b" ) );
            var iterator = CreateIterator();
            iterator.CurrentNode = page;

            bool value = iterator.Defined( "page.type" );

            Assert.IsFalse( value );
        }

        [Test]
        public void Defined_PageAttributeIsDefinedOnPage_ReturnsTrue()
        {
            var page = new PageBody( PageName.Create( "a" ), new PageAttribute( "page", "type", "note" ) );
            var iterator = CreateIterator();
            iterator.CurrentNode = page;

            bool value = iterator.Defined( "page.type" );

            Assert.IsTrue( value );
        }


        [Test]
        public void All_PageAttributeIsOnlyReferenced_ReturnsFalse()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PageAttribute( "page", "type" );

            bool value = iterator.All( "page.type" );

            Assert.IsFalse( value );
        }

        [Test]
        public void All_NotAPageAttribute_ReturnsFalse()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PlainText( "b" );

            bool value = iterator.All( "page.type" );

            Assert.IsFalse( value );
        }

        [Test]
        public void All_PageAttributeIsDefinition_ReturnsTrue()
        {
            var iterator = CreateIterator();
            iterator.CurrentNode = new PageAttribute( "page", "type", "note" );

            bool value = iterator.All( "page.type" );

            Assert.IsTrue( value );
        }

        private QueryIterator CreateIterator()
        {
            var query = MockFactory.CreateCompiledQuery( "true" );
            return new QueryIterator( query );
        }
    }
}
