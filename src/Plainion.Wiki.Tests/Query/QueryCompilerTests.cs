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
    public class QueryCompilerTests
    {
        private QueryCompiler myCompiler;
        private const string myAnyValidExpression = "true";

        [SetUp]
        public void SetUp()
        {
            myCompiler = new QueryCompiler();
        }

        [Test]
        public void Compile_EmptySelect_ShouldResultInPageSelectClause()
        {
            var def = new QueryDefinition( myAnyValidExpression, string.Empty );

            var query = myCompiler.Compile( def );

            Assert.That( query.SelectClause, Is.InstanceOf<PageSelectClause>() );
        }

        [Test]
        public void Compile_EmptyFrom_ShouldResultInPageSelectClause()
        {
            var def = new QueryDefinition( myAnyValidExpression, string.Empty, string.Empty );

            var query = myCompiler.Compile( def );

            Assert.That( query.FromClause, Is.InstanceOf<AlwaysTrueFromClause>() );
        }

        [Test]
        public void Compile_NonEmptyWhere_ShouldResultInDynamicLinqWhereClause()
        {
            var def = new QueryDefinition( myAnyValidExpression );

            var query = myCompiler.Compile( def );

            Assert.That( query.WhereClause, Is.InstanceOf<DynamicLinqWhereClause>() );
        }

        [Test]
        public void Compile_SectionSelect_ShouldResultInSectionSelectClause()
        {
            var def = new QueryDefinition( myAnyValidExpression, "section" );

            var query = myCompiler.Compile( def );

            Assert.That( query.SelectClause, Is.InstanceOf<SectionSelectClause>() );
        }

        [Test]
        public void Compile_ParentSelect_ShouldResultInParentSelectClause()
        {
            var def = new QueryDefinition( myAnyValidExpression, "parent" );

            var query = myCompiler.Compile( def );

            Assert.That( query.SelectClause, Is.InstanceOf<ParentSelectClause>() );
        }

        [Test]
        public void Compile_AttributeValueSelect_ShouldResultInAttributeValueSelectClause()
        {
            var def = new QueryDefinition( myAnyValidExpression, "attribute-value" );

            var query = myCompiler.Compile( def );

            Assert.That( query.SelectClause, Is.InstanceOf<AttributeValueSelectClause>() );
        }

        [Test]
        public void Compile_DownOnlyFromAndQueryDefWithoutPageBody_Throws()
        {
            var def = new QueryDefinition( myAnyValidExpression, string.Empty, "down-only" );

            Assert.Throws<InvalidOperationException>( () => myCompiler.Compile( def ) );
        }

        [Test]
        public void Compile_DownOnlyFrom_ShouldResultInNoParentFromClause()
        {
            var def = new QueryDefinition( myAnyValidExpression, string.Empty, "down-only" );
            var body = new PageBody( PageName.Create( "a" ) );
            body.Consume( def );

            var query = myCompiler.Compile( def );

            Assert.That( query.FromClause, Is.InstanceOf<NoParentFromClause>() );
        }
    }
}
