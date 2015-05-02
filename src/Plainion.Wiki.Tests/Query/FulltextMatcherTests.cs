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
    public class FulltextMatcherTests
    {
        [Test]
        public void Ctor_WhenCalled_SearchTextPropertyIsSet()
        {
            var matcher = new FulltextMatcher( "a" );

            Assert.That( matcher.SearchText, Is.EqualTo( "a" ) );
        }

        [Test]
        public void Ctor_WithEmptySearchStrings_SearchTextPropertyIsSet( [Values( null, "", "  \t   " )] string searchText )
        {
            var matcher = new FulltextMatcher( searchText );

            Assert.That( matcher.SearchText, Is.Null );
        }

        [Test]
        public void Ctor_WithStringWithWhitespaces_SearchStringIsNotTrimmed( [Values( " a", "a ", "\tx\t" )] string searchText )
        {
            var matcher = new FulltextMatcher( searchText );

            Assert.That( matcher.SearchText, Is.EqualTo( searchText ) );
        }

        [Test]
        public void Match_WithEmptySearchString_Matches()
        {
            var page = MockFactory.CreatePage( PageName.Create( "a" ), "abc def ghi", "xyz", "12 34 56" );
            var matcher = new FulltextMatcher( null );

            var matches = matcher.Match( page );

            XAssert.IsPageMatchOfPage( matches.Single(), page.Name );
        }

        [Test]
        public void Match_WithMatchingPageContent_ReturnsPageMatch()
        {
            var page = MockFactory.CreatePage( PageName.Create( "a" ), "abc def ghi", "xyz", "12 34 56" );
            var matcher = new FulltextMatcher( "34" );

            var matches = matcher.Match( page );

            XAssert.IsPageMatchOfPage( matches.Single(), page.Name );
        }

        [Test]
        public void Match_PageDoesNotMatch_ReturnsEmptySet()
        {
            var page = MockFactory.CreatePage( PageName.Create( "a" ), "abc def ghi", "xyz", "12 34 56" );
            var matcher = new FulltextMatcher( "00" );

            var matches = matcher.Match( page );

            Assert.That( matches, Is.Empty );
        }

        [Test]
        public void Match_PageNameMatches_ReturnsPageMatch()
        {
            var page = MockFactory.CreatePage( PageName.Create( "a bc d" ), "123", "456" );
            var matcher = new FulltextMatcher( "bc" );

            var matches = matcher.Match( page );

            XAssert.IsPageMatchOfPage( matches.Single(), page.Name );
        }
    }
}
