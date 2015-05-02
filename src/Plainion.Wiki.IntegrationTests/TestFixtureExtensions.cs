
namespace Plainion.Wiki.IntegrationTests
{
    public static class TestFixtureExtensions
    {
        public static TestEnvironment TestEnvironment( this object testFixture )
        {
            return new TestEnvironment( testFixture.GetType() );
        }
    }
}
