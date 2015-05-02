using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Parser;
using Moq;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.DataAccess
{
    [TestFixture]
    public class PageRepositoryTest
    {
        private Mock<IPageAccess> myPageAccess;
        private ParserPipeline myParser;
        private PageRepository myRepository;

        [SetUp]
        public void SetUp()
        {
            myPageAccess = new Mock<IPageAccess>() { DefaultValue = DefaultValue.Mock };
            myParser = new ParserPipeline();

            myRepository = new PageRepository( myPageAccess.Object, myParser );
        }

        [TearDown]
        public void TearDown()
        {
            myRepository = null;
            myParser = null;
            myPageAccess = null;
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithoutPageAccess()
        {
            new PageRepository( null, myParser );
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithoutPageParser()
        {
            new PageRepository( myPageAccess.Object, null );
        }

        [Test]
        public void ByPassPagesToPageAccess()
        {
            CollectionAssert.IsEmpty( myRepository.Pages );

            myPageAccess.SetupGet( x => x.Pages ).Returns( new[] { new InMemoryPageDescriptor( PageName.Create( "Page1" ) ) } );

            CollectionAssert.AreEqual( myPageAccess.Object.Pages, myRepository.Pages );
        }

        [Test]
        public void ByPassFindToPageAccess()
        {
            var pageName = PageName.Create( "Page1" );

            myPageAccess.Setup( x => x.Find( pageName ) ).Returns<IPageDescriptor>( null );
            Assert.IsNull( myRepository.Find( pageName ) );

            var descriptor = new InMemoryPageDescriptor( pageName );
            myPageAccess.Setup( x => x.Find( pageName ) ).Returns( descriptor );

            Assert.AreEqual( descriptor, myRepository.Find( pageName ) );
        }

        [Test]
        public void ByPassCreateToPageAccess()
        {
            myRepository.Create( new InMemoryPageDescriptor( PageName.Create( "Page1" ) ) );
            myRepository.Create( PageName.Create( "Page2" ), new[] { "line1", "line2" } );

            myPageAccess.Verify( x => x.Create( It.IsAny<IPageDescriptor>() ), Times.Exactly( 2 ) );
        }

        [Test]
        public void ByPassDeleteToPageAccess()
        {
            myRepository.Delete( new InMemoryPageDescriptor( PageName.Create( "Page1" ) ) );

            myPageAccess.Verify( x => x.Delete( It.IsAny<IPageDescriptor>() ), Times.Once() );
        }

        [Test]
        public void CleanupCacheOnDelete()
        {
            AssertCacheCleanupOnAction( descriptor => myRepository.Delete( descriptor ) );
        }

        [Test]
        public void DeleteByPageName()
        {
            var pageName = PageName.Create( "Page1" );

            var descriptor = new InMemoryPageDescriptor( pageName );
            myPageAccess.Setup( x => x.Find( pageName ) ).Returns( descriptor );

            myRepository.Delete( pageName );

            myPageAccess.Verify( x => x.Delete( It.IsAny<IPageDescriptor>() ), Times.Once() );
        }

        [Test]
        public void CleanupCacheOnDeleteByPageName()
        {
            AssertCacheCleanupOnAction( descriptor => myRepository.Delete( descriptor.Name ) );
        }

        [Test]
        public void ByPassUpdateToPageAccess()
        {
            myRepository.Update( new InMemoryPageDescriptor( PageName.Create( "Page1" ) ) );
            myRepository.Update( PageName.Create( "Page2" ), new[] { "line1", "line2" } );

            myPageAccess.Verify( x => x.Update( It.IsAny<IPageDescriptor>() ), Times.Exactly( 2 ) );
        }

        [Test]
        public void CleanupCacheOnUpdate()
        {
            AssertCacheCleanupOnAction( descriptor => myRepository.Update( descriptor ) );
        }

        [Test]
        public void CleanupCacheOnUpdateByPageName()
        {
            AssertCacheCleanupOnAction( descriptor => myRepository.Update( descriptor.Name, new[] { "line1" } ) );
        }

        [Test]
        public void GetPageByPageName()
        {
            var pageName = PageName.Create( "Page1" );

            var descriptor = new InMemoryPageDescriptor( pageName );
            myPageAccess.Setup( x => x.Find( pageName ) ).Returns( descriptor );

            var page = myRepository.Get( pageName );

            Assert.AreEqual( descriptor.Name, page.Name );
        }

        [Test]
        public void GetNullIfPageNotExists()
        {
            var pageName = PageName.Create( "Page1" );

            myPageAccess.Setup( x => x.Find( pageName ) ).Returns<IPageDescriptor>( null );

            Assert.IsNull( myRepository.Get( pageName ) );
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetWithNullDescriptor()
        {
            myRepository.Get( (IPageDescriptor)null );
        }

        [Test]
        public void FillCacheOnGet()
        {
            var pageName = PageName.Create( "Page1" );

            var descriptor = new InMemoryPageDescriptor( pageName );
            myPageAccess.Setup( x => x.Find( pageName ) ).Returns( descriptor );

            var page = myRepository.Get( pageName );

            Assert.AreEqual( page, myRepository.Get( pageName ),
               "Cache not filled properly. Got different instance back" );
        }

        // No longer valid - we have to accept pages not in pageacces because
        // of in-memory-generated pages like "PageNotFound"
        //[Test]
        //public void DenyPagesUnavailableToPageAccess()
        //{
        //    var pageName = PageName.Create( "Page1" );
        //
        //    myMockery.Return( myPageAccess.Find( pageName ), null );
        //
        //    Assert.IsNull( myRepository.Get( new InMemoryPageDescriptor( pageName ) ) );
        //}

        private void AssertCacheCleanupOnAction( Action<IPageDescriptor> action )
        {
            var pageName = PageName.Create( "Page1" );

            // prepare PageAccess
            var descriptor = new InMemoryPageDescriptor( pageName );
            myPageAccess.Setup( x => x.Find( pageName ) ).Returns( descriptor );

            // fills the cache
            var cachedPage = myRepository.Get( descriptor );
            var cachedPageForSelfTest = myRepository.Get( descriptor );

            // self-test
            Assert.AreEqual( cachedPage, cachedPageForSelfTest,
                "We must get the same instance when asking the cache twice for the same data" );

            action( descriptor );

            Assert.AreNotEqual( cachedPage, myRepository.Get( descriptor ),
                "Cache not cleaned up properly. Got same instance back" );
        }
    }
}
