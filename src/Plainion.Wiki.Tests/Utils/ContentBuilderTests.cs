using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.Query;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.UnitTests.Utils
{
    [TestFixture]
    public class ContentBuilderTests
    {
        [Test]
        public void BuildQueryResultNoBullets_NoResults_ContentContainsNoResults()
        {
            var hits = new List<QueryMatch>();

            var content = ContentBuilder.BuildQueryResultNoBullets( hits, "n.a." );

            var expectedContent = (PlainText)content.Children.Single();
            Assert.That( expectedContent.Text, Is.EqualTo( "n.a." ) );
        }

        [Test]
        public void BuildQueryResultNoBullets_WithResults_ContentContainsResults()
        {
            var hits = new List<QueryMatch>();
            var match = new PlainText( "a" );
            hits.Add( new QueryMatch( match ) );

            var content = ContentBuilder.BuildQueryResultNoBullets( hits, "" );

            Assert.That( content.Children, Contains.Item( match ) );
        }

        [Test]
        public void BuildQueryResult_NoResults_ContentContainsNoResults()
        {
            var hits = new List<QueryMatch>();

            var content = ContentBuilder.BuildQueryResult( hits, "n.a." );

            var expectedContent = (PlainText)content.Children.Single();
            Assert.That( expectedContent.Text, Is.EqualTo( "n.a." ) );
        }

        [Test]
        public void BuildQueryResult_WithResults_ContentContainsResults()
        {
            var hits = new List<QueryMatch>();
            var match = new PlainText( "a" );
            hits.Add( new QueryMatch( match ) );

            var content = ContentBuilder.BuildQueryResult( hits );

            var listItem = (ListItem)content.Children.Single();
            Assert.That( listItem.Text.Text(), Is.EqualTo( "a" ) );
        }
    }
}
