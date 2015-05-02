using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Parser;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class QueryParserTests
    {
        [Test]
        public void Parse_OneExpression_WhereExpressionWillBeSet()
        {
            var parser = new QueryParser();

            // use multi-char string
            var query = parser.Parse( "123" );

            Assert.That( query.WhereExpression, Is.EqualTo( "123" ) );
            Assert.That( query.SelectExpression, Is.EqualTo( string.Empty ) );
            Assert.That( query.FromExpression, Is.EqualTo( string.Empty ) );
        }

        [Test]
        public void Parse_TwoExpressions_WhereAndSelectExpressionsWillBeSet()
        {
            var parser = new QueryParser();

            // use multi-char string
            var query = parser.Parse( "123;abc" );

            Assert.That( query.WhereExpression, Is.EqualTo( "123" ) );
            Assert.That( query.SelectExpression, Is.EqualTo( "abc" ) );
            Assert.That( query.FromExpression, Is.EqualTo( string.Empty ) );
        }

        [Test]
        public void Parse_ThreeExpressions_WhereAndSelectAndFromExpressionsWillBeSet()
        {
            var parser = new QueryParser();

            // use multi-char string
            var query = parser.Parse( "a1;b2;c3" );

            Assert.That( query.WhereExpression, Is.EqualTo( "a1" ) );
            Assert.That( query.SelectExpression, Is.EqualTo( "b2" ) );
            Assert.That( query.FromExpression, Is.EqualTo( "c3" ) );
        }

        [Test]
        public void Parse_MoreThanThreeExpressions_Throws()
        {
            var parser = new QueryParser();

            Assert.Throws<Exception>( () => parser.Parse( "a;b;c;d" ) );
        }

        [Test]
        public void Parse_CalledTwice_OldValuesAreResetted()
        {
            var parser = new QueryParser();

            parser.Parse( "a;b" );
            var query = parser.Parse( "x" );

            Assert.That( query.WhereExpression, Is.EqualTo( "x" ) );
            Assert.That( query.SelectExpression, Is.EqualTo( string.Empty ) );
            Assert.That( query.FromExpression, Is.EqualTo( string.Empty ) );
        }

        [Test]
        public void Parse_OneExpressionWithEscapedSemiColon_OnlyWhereExpressionWillBeSet()
        {
            var parser = new QueryParser();

            // use multi-char string
            var query = parser.Parse( @"123\;abc" );

            Assert.That( query.WhereExpression, Is.EqualTo( @"123;abc" ) );
            Assert.That( query.SelectExpression, Is.EqualTo( string.Empty ) );
            Assert.That( query.FromExpression, Is.EqualTo( string.Empty ) );
        }
    }
}
