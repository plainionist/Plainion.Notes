using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;
using Plainion.Wiki.DataAccess.FlatFile;
using System;
using System.IO;
using System.Threading;
using Plainion.Wiki.DataAccess;
using Plainion.IO;
using Plainion.IO.RealFS;

namespace Plainion.Wiki.UnitTests.DataAccess.FlatFile
{
    [TestFixture]
    public class FlatFilePageAccessTest
    {
        private class MockPageHistoryAccess : IPageHistoryAccess
        {
            public int CreateVersionCalls
            {
                get;
                set;
            }

            public void CreateNewVersion( IPageDescriptor pageDescriptor )
            {
                CreateVersionCalls++;
            }
        }

        private MockPageHistoryAccess myHistoryAccess;
        private IFileSystem myFileSystem;
        private IDirectory mySampleRepository;

        [SetUp]
        public void SetUp()
        {
            myFileSystem = new FileSystemImpl();

            CleanupSampleRepository();
            CreateSampleRepository();

            myHistoryAccess = new MockPageHistoryAccess();
        }

        [TearDown]
        public void TearDown()
        {
            CleanupSampleRepository();
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithoutRepository()
        {
            new FlatFilePageAccess( null, new MockPageHistoryAccess() );
        }

        [Test]
        [ExpectedException( typeof( FileNotFoundException ) )]
        public void CreateWithInvalidRepository()
        {
            new FlatFilePageAccess( myFileSystem.Directory( "DirDoesNotExist" ), new MockPageHistoryAccess() );
        }

        [Test]
        public void CreateWithEmptyRepository()
        {
            var access = CreateDefaultAccess( mySampleRepository );

            Assert.AreEqual( mySampleRepository, access.PageRepository );
            Assert.AreEqual( 0, access.Pages.Count() );
        }

        [Test]
        public void CreateWithRepository()
        {
            mySampleRepository = PrepareSampleRepository();

            var access = CreateDefaultAccess( mySampleRepository );

            Assert.AreEqual( mySampleRepository, access.PageRepository );
            Assert.AreEqual( 3, access.Pages.Count() );
        }

        [Test]
        public void Find()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test1" );
            var page = access.Find( pageName );
            Assert.AreEqual( pageName, page.Name );

            page = access.Find( PageName.Create( "PageDoesNotExist" ) );
            Assert.IsNull( page );
        }

        [Test]
        public void Create()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test_3" );
            access.Create( new InMemoryPageDescriptor( pageName, "line1", "line2" ) );
            Assert.AreEqual( 0, myHistoryAccess.CreateVersionCalls );

            Assert_PageContentEquals( access, pageName, "line1", "line2" );

            // check with new instance because of caching
            access = CreateDefaultAccess( mySampleRepository );

            Assert_PageContentEquals( access, pageName, "line1", "line2" );
        }

        [Test]
        public void CreateAlreadyExistingPageWillUpdateIt()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test2" );
            var page = access.Find( pageName );

            access.Create( new InMemoryPageDescriptor( page.Name, "line1", "line75" ) );
            Assert.AreEqual( 1, myHistoryAccess.CreateVersionCalls );

            Assert_PageContentEquals( access, pageName, "line1", "line75" );

            // check with new instance because of caching
            access = CreateDefaultAccess( mySampleRepository );

            Assert_PageContentEquals( access, pageName, "line1", "line75" );
        }

        [Test]
        [ExpectedException( typeof( NotSupportedException ) )]
        public void CreateWithInvalidPageName()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            access.Create( new InMemoryPageDescriptor( PageName.Create( "invalid:file" ), "line1", "line2" ) );
        }

        [Test]
        public void UpdateShouldOverwritePage()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test2" );
            var page = access.Find( pageName );
            var updatedPage = new InMemoryPageDescriptor( pageName, page.GetContent() );
            updatedPage.AddContent( "one more line" );

            access.Update( updatedPage );
            Assert.AreEqual( 1, myHistoryAccess.CreateVersionCalls );

            CheckLastNLines( access, pageName, "one more line" );

            // check with new instance because of caching
            access = CreateDefaultAccess( mySampleRepository );

            CheckLastNLines( access, pageName, "one more line" );
        }

        [Test]
        public void UpdateOnNonExistentPageWillCreateOne()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test75" );
            var page = new InMemoryPageDescriptor( pageName, "line1", "line75" );

            access.Update( page );
            Assert.AreEqual( 0, myHistoryAccess.CreateVersionCalls );

            Assert_PageContentEquals( access, pageName, "line1", "line75" );

            // check with new instance because of caching
            access = CreateDefaultAccess( mySampleRepository );

            Assert_PageContentEquals( access, pageName, "line1", "line75" );
        }

        [Test]
        public void Delete()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test2" );
            var page = access.Find( pageName );

            access.Delete( page );

            // as we do update on write we need to create a backup of the most recent version of the file
            Assert.AreEqual( 1, myHistoryAccess.CreateVersionCalls );

            Assert.IsNull( access.Find( pageName ) );

            // check with new instance because of caching
            access = CreateDefaultAccess( mySampleRepository );

            Assert.IsNull( access.Find( pageName ) );
        }

        [Test]
        public void DeleteWithInMemoryPageDescriptor()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var pageName = PageName.Create( "test2" );

            access.Delete( new InMemoryPageDescriptor( pageName ) );

            // as we do update on write we need to create a backup of the most recent version of the file
            Assert.AreEqual( 1, myHistoryAccess.CreateVersionCalls );

            Assert.IsNull( access.Find( pageName ) );
        }

        [Test]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void DeleteNonExistentFileWillFail()
        {
            var access = CreateDefaultAccess( PrepareSampleRepository() );

            var page = new InMemoryPageDescriptor( PageName.Create( "test75" ), "line1", "line75" );

            access.Delete( page );
        }

        [Test]
        public void CreateAndFindPageNameWithPath()
        {
            var access = CreateDefaultAccess( mySampleRepository );

            var pageName = PageName.CreateFromPath( "/Tools/Overview" );
            access.Create( new InMemoryPageDescriptor( pageName, "line1", "line2" ) );

            Assert_PageContentEquals( access, pageName, "line1", "line2" );

            // check with new instance because of caching
            access = CreateDefaultAccess( mySampleRepository );

            Assert_PageContentEquals( access, pageName, "line1", "line2" );
        }

        private FlatFilePageAccess CreateDefaultAccess( IDirectory repository )
        {
            return new FlatFilePageAccess( repository, myHistoryAccess );
        }

        private void CleanupSampleRepository()
        {
            if ( mySampleRepository != null && mySampleRepository.Exists )
            {
                Thread.Sleep( 1 );
                mySampleRepository.Delete( true );
            }
        }

        private void CreateSampleRepository()
        {
            mySampleRepository = myFileSystem.GetTempPath().Directory( "TestRepo" );
            mySampleRepository.Create();
        }

        private IDirectory PrepareSampleRepository()
        {
            var access = new FlatFilePageAccess( mySampleRepository, myHistoryAccess );

            access.Create( new InMemoryPageDescriptor( PageName.Create( "test1" ), "line1" ) );
            access.Create( new InMemoryPageDescriptor( PageName.Create( "test2" ), "line2" ) );
            access.Create( new InMemoryPageDescriptor( PageName.Create( "test3" ), "line3" ) );

            return mySampleRepository;
        }

        private void Assert_PageContentEquals( IPageAccess access, PageName pageName, params string[] lines )
        {
            var page = access.Find( pageName );
            var content = page.GetContent();

            Assert.AreEqual( lines.Length, content.Length );

            for ( int i = 0; i < lines.Length; ++i )
            {
                Assert.AreEqual( lines[ i ], content[ i ] );
            }
        }

        private void CheckLastNLines( IPageAccess access, PageName pageName, params string[] lines )
        {
            var page = access.Find( pageName );
            var content = page.GetContent();

            Assert.GreaterOrEqual( content.Length, lines.Length );

            int offset = content.Length - lines.Length;
            for ( int i = 0; i < lines.Length; ++i )
            {
                Assert.AreEqual( lines[ i ], content[ offset + i ] );
            }
        }
    }
}
