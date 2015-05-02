using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class PageCacheTest_FilledCache
    {
        private PageCache myCache;
        private PageBody myExistingPage;

        [SetUp]
        public void SetUp()
        {
            myCache = new PageCache();
            myExistingPage = new PageBody( PageName.Create( "NewPage" ) );
            myCache.Add( myExistingPage );
        }

        [TearDown]
        public void TearDown()
        {
            myCache = null;
        }

        [Test]
        public void OverwriteExistingPage()
        {
            myCache.Add( myExistingPage );

            CollectionAssert.AreEqual( new[] { myExistingPage.Name }, myCache.Pages );
        }

        [Test]
        public void FindExistingPage()
        {
            var page = myCache.FindByName( myExistingPage.Name );

            Assert.AreEqual( myExistingPage, page );
        }

        [Test]
        public void RemoveExistingPage()
        {
            myCache.Remove( myExistingPage.Name );

            CollectionAssert.IsEmpty( myCache.Pages );
        }

        [Test]
        public void Clear_WhenCalled_CacheWillBeCleared()
        {
            var page = new PageBody( PageName.Create( "NewPage" ) );
            myCache.Add( page );

            myCache.Clear();

            Assert.That( myCache.Pages, Is.Empty );
        }
    }
}
