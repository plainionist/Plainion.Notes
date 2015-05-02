using NUnit.Framework;
using Plainion.Testing;

namespace Plainion.Wiki.UnitTests
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
