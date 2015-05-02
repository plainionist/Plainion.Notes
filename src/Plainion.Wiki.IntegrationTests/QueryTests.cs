using Plainion.Testing;
using Plainion.Wiki.IntegrationTests.Tasks;
using NUnit.Framework;

namespace Plainion.Wiki.IntegrationTests
{
    [TestFixture]
    public class QueryTests : TestBase
    {
        [Test]
        public void Where_AttributeEqualsString()
        {
            var page1 = Engine.CreatePage( "Page1", "[@page.type: note]" );
            var page2 = Engine.CreatePage( "Page2", "[@page.type: note]" );
            var page3 = Engine.CreatePage( "Page3", "[@page.type: someThingOther]" );
            var page4 = Engine.CreatePage( "Page4", "[@query: page.type == \"note\"]" );

            var output = Engine.Render( page4 );

            var expected = LoadTestData( "Query_Where_AttributeEqualsString_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Where_PageLinkedByPageOfQuery()
        {
            var page1 = Engine.CreatePage( "Page1", "this is page one" );
            var page2 = Engine.CreatePage( "Page2", "this is page two" );
            var page3 = Engine.CreatePage( "Page3", "[Page1]", "[@query: linked()]" );

            var output = Engine.Render( page3 );

            var expected = LoadTestData( "Query_Where_PageLinkedByPageOfQuery_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Where_AttributeDefinedOnPage()
        {
            var page1 = Engine.CreatePage( "Page1", "[@page.type: note]" );
            var page2 = Engine.CreatePage( "Page2", "[@page.type: someThingOther]" );
            var page3 = Engine.CreatePage( "Page3", "[@query: defined(\"page.type\")]" );

            var output = Engine.Render( page3 );

            var expected = LoadTestData( "Query_Where_AttributeDefinedOnPage_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Where_AttributeEqualsStringAndLinkedFunction()
        {
            var page1 = Engine.CreatePage( "Page1", "[@page.type: note]" );
            var page2 = Engine.CreatePage( "Page2", "[@page.type: note]" );
            var page3 = Engine.CreatePage( "Page3", "[@page.type: note]" );
            var page4 = Engine.CreatePage( "Page4", "[Page1]", "[Page3]", "[@query: linked() && page.type == \"note\"]" );

            var output = Engine.Render( page4 );

            var expected = LoadTestData( "Query_Where_AttributeEqualsStringAndLinkedFunction_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Where_AttributeDefinedMultipleTimesOnPage_SelectPage()
        {
            var page1 = Engine.CreatePage( "Page1", "[@asap:]", "[@asap:]" );
            var page2 = Engine.CreatePage( "Page2", "[@query: defined(\"asap\")]" );

            var output = Engine.Render( page2 );

            var expected = LoadTestData( "Query_Where_AttributeDefinedMultipleTimesOnPage_SelectPage_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Where_AttributeDefinedMultipleTimesOnPage_SelectParent()
        {
            var page1 = Engine.CreatePage( "Page1", "* action item 1 [@asap:]", "* action item 2 [@asap:]" );
            var page2 = Engine.CreatePage( "Page2", "[@query: all(\"asap\");parent]" );

            var output = Engine.Render( page2 );

            var expected = LoadTestData( "Query_Where_AttributeDefinedMultipleTimesOnPage_SelectParent_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Select_AttributeValue()
        {
            var page1 = Engine.CreatePage( "Page1", "[@page.type: note]" );
            var page2 = Engine.CreatePage( "Page2", "[@query: defined(\"page.type\");attribute-value]" );

            var output = Engine.Render( page2 );

            var expected = LoadTestData( "Query_Select_AttributeValue_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Select_Section()
        {
            var page1 = Engine.CreatePage( "Page1", "!!! SectionP1", "[@page.type: note]" );
            var page2 = Engine.CreatePage( "Page2", "[@query: all(\"page.type\");section]" );

            var output = Engine.Render( page2 );

            var expected = LoadTestData( "Query_Select_Section_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void Select_Parent()
        {
            var page1 = Engine.CreatePage( "Page1", "* action item [@asap:]" );
            var page2 = Engine.CreatePage( "Page2", "[@query: all(\"asap\");parent]" );

            var output = Engine.Render( page2 );

            var expected = LoadTestData( "Query_Select_Parent_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }

        [Test]
        public void From_DownOnly()
        {
            var page1 = Engine.CreatePage( "/Project1/Page1", "[@asap:]" );
            var page2 = Engine.CreatePage( "/Project2/Page2", "[@asap:]" );
            var page3 = Engine.CreatePage( "/Project1/Page2", "[@query: defined(\"asap\");;down-only]" );

            var output = Engine.Render( page3 );
            //output.Foreach( Console.WriteLine );

            var expected = LoadTestData( "Query_From_DownOnly_expected.txt" );
            BAssert.TextSemanticallyEquals( expected, output );
        }
    }
}
