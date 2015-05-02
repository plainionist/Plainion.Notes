using Plainion.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.IntegrationTests
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        [SetUp]
        public void SetUp()
        {
            var ignore = new BinaryFormatterInitializer();
        }
    }
}
