using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Query;
using System.Linq.Expressions;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class QueryIdentifierResolverTests
    {
        private QueryIdentifierResolver myResolver;

        [SetUp]
        public void SetUp()
        {
            myResolver = new QueryIdentifierResolver();
        }

        [Test]
        public void HasValue_NullIdentifier_ReturnsFalse( [Values( null, "" )] string identifier )
        {
            bool hasValue = myResolver.HasValue( identifier );

            Assert.IsFalse( hasValue );
        }

        [Test]
        public void HasValue_QualifiedIdentifier_ReturnsTrue()
        {
            bool hasValue = myResolver.HasValue( "page.type" );

            Assert.IsTrue( hasValue );
        }

        [Test]
        public void HasValue_PrefixedIdentifier_ReturnsTrue()
        {
            bool hasValue = myResolver.HasValue( "@asap" );

            Assert.IsTrue( hasValue );
        }

        [Test]
        public void GetValue_ValidIdentifier_ReturnMethodCallToIterator()
        {
            var iterator = Expression.Parameter( typeof( IQueryIterator ), "" );

            var value = myResolver.GetValue( iterator, "@asap" );

            Assert.That( value, Is.InstanceOf<MethodCallExpression>() );
            var methodCall = (MethodCallExpression)value;
            var expectedIteratorMethod = typeof( IQueryIterator ).GetMethod( "GetIdentifierValue" );
            Assert.That( methodCall.Method.Name, Is.EqualTo( expectedIteratorMethod.Name ) );
        }
    }
}
