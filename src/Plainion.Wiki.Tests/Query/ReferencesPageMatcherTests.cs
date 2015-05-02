using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Query;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;
using Plainion.Wiki.UnitTests.Testing;

namespace Plainion.Wiki.UnitTests.Query
{
    [TestFixture]
    public class ReferencesPageMatcherTests
    {
        [Test]
        public void Match_PageNotReferenced_ReturnsEmptySet()
        {
            var referencedPage = PageName.Create( "x" );
            var page = MockFactory.CreatePage( PageName.Create( "a" ) );
            var matcher = new ReferencesPageMatcher( referencedPage );

            var matches = matcher.Match( page );

            Assert.That( matches, Is.Empty );
        }

        [Test]
        public void Match_PageReferenced_ReturnsEmptySet()
        {
            var referencedPage = PageName.Create( "x" );
            var page = MockFactory.CreatePage( PageName.Create( "a" ) );
            page.Body.Consume( new Link( referencedPage ) );
            var matcher = new ReferencesPageMatcher( referencedPage );

            var matches = matcher.Match( page );

            XAssert.IsPageMatchOfPage( matches.Single(), page.Name );
        }
    }
}
