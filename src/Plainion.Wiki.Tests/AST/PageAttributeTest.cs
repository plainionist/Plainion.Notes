using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;
using System;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class PageAttributeTest
    {
        [Test]
        public void Ctor_WithNullType_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new PageAttribute( null, "name" ) );
        }

        [Test]
        public void Ctor_WithNullName_IsAccepted()
        {
            var attr = new PageAttribute( "query", null );

            Assert.AreEqual( "query", attr.Type );
            Assert.IsNull( attr.Name );
            Assert.IsNull( attr.Value );
            Assert.AreEqual( "query", attr.FullName );
        }

        [Test]
        public void AttributeReferenceWithTypeAndName()
        {
            var attr = new PageAttribute( "page", "type" );

            Assert.AreEqual( "page", attr.Type );
            Assert.AreEqual( "type", attr.Name );
            Assert.IsNull( attr.Value );
            Assert.AreEqual( "page.type", attr.FullName );
        }

        [Test]
        public void AttributeDefinitionWithTypeAndName()
        {
            var attr = new PageAttribute( "page", "type", "stock" );

            Assert.AreEqual( "page", attr.Type );
            Assert.AreEqual( "type", attr.Name );
            Assert.AreEqual( "stock", attr.Value );
            Assert.AreEqual( "page.type", attr.FullName );
        }

        [Test]
        public void TrimTypeAndNameAndValue()
        {
            var attr = new PageAttribute( " page ", " type ", " stock " );

            Assert.AreEqual( "page", attr.Type );
            Assert.AreEqual( "type", attr.Name );
            Assert.AreEqual( "stock", attr.Value );
            Assert.AreEqual( "page.type", attr.FullName );
        }

        [Test]
        public void IsDefinition_ForDefinition_ReturnsTrue()
        {
            var attr = new PageAttribute( "page", "type", "stock" );

            Assert.IsTrue( attr.IsDefinition );
        }
    }
}
