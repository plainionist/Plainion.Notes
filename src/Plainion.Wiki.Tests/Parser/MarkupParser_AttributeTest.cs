using Plainion.Wiki.AST;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Parser
{
    [TestFixture]
    public class MarkupParser_AttributeTest : MarkupParser_TestBase
    {
        [Test]
        public void Parse_AttributeDefinitionWithoutValue_ShouldBeInterpretedAsDefinition()
        {
            var attr = "[@gtd.asap:]";

            Parse( attr );

            Assert_OutputEquals( new PageAttribute( "gtd", "asap", string.Empty ) );
        }

        //[Test]
        //public void Parse_HeadlineWithAttributeReference_AttributeShouldBeDetected()
        //{
        //    var page = "!!! [@projects.current]";

        //    Parse( page );

        //    Assert_OutputEquals( new PageAttribute( "gtd", "asap", string.Empty ) );
        //}
    }
}
