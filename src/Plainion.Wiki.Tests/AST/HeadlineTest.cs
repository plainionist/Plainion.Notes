using Plainion.Wiki.AST;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.AST
{
    [TestFixture]
    public class HeadlineTest
    {
        [Test]
        public void Creation()
        {
            var headline = new Headline( "headline", 3 );

            Assert.AreEqual( "headline", headline.Text );
            Assert.AreEqual( 3, headline.Size );
        }

        [Test]
        public void AnchorAndWhitespaces()
        {
            var headline = new Headline( "A headline with whitespaces", 3 );

            Assert.AreEqual( "Aheadlinewithwhitespaces", headline.Anchor );
        }
    }
}
