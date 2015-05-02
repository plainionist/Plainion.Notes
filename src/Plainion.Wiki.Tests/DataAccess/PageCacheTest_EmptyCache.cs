using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class PageCacheTest_EmptyCache
    {
        private PageCache myCache;

        [SetUp]
        public void SetUp()
        {
            myCache = new PageCache();
        }

        [TearDown]
        public void TearDown()
        {
            myCache = null;
        }

        [Test]
        public void CacheIsEmptyAfterCreation()
        {
            CollectionAssert.IsEmpty( myCache.Pages );
        }

        [Test]
        public void AddNotExistingPage()
        {
            var page = new PageBody( PageName.Create( "NewPage" ) );
            myCache.Add( page );

            CollectionAssert.AreEqual( new[] { page.Name }, myCache.Pages );
        }

        [Test]
        public void FindNonExistingPage()
        {
            Assert.IsNull( myCache.FindByName( PageName.Create( "NotExistingPage" ) ) );
        }

        [Test]
        public void IgnoreRemovalOfNonExistingPage()
        {
            myCache.Remove( PageName.Create( "NotExistingPage" ) );

            CollectionAssert.IsEmpty( myCache.Pages );
        }
    }
}
