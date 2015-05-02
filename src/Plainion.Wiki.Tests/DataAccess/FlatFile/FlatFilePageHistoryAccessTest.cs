using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Plainion.IO;
using Plainion.IO.RealFS;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess.FlatFile;
using NUnit.Framework;
using Plainion;

namespace Plainion.Wiki.UnitTests.DataAccess.FlatFile
{
    [TestFixture]
    public class FlatFilePageHistoryAccessTest
    {
        private IFileSystem myFileSystem;
        private IDirectory mySampleRepository;

        [SetUp]
        public void SetUp()
        {
            myFileSystem = new FileSystemImpl();

            CleanupSampleRepository();
            CreateSampleRepository();
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
            new FlatFilePageHistoryAccess( null );
        }

        [Test]
        public void Creation()
        {
            var access = new FlatFilePageHistoryAccess( mySampleRepository );

            Assert.AreEqual( mySampleRepository, access.HistoryRoot );
        }

        // Each CreateVersion call should create a new version file until 
        // the MaxVersions count has been reached.
        [Test]
        public void CreateVersionCreatesNewVersionFile()
        {
            var access = new FlatFilePageHistoryAccess( mySampleRepository );
            var pageName = PageName.Create( "test" );
            var page = CreateOrUpdateTestPage( pageName, "one" );

            access.CreateNewVersion( page );
            Assert.AreEqual( 1, GetVersions( pageName ).Count() );

            access.CreateNewVersion( page );
            Assert.AreEqual( 2, GetVersions( pageName ).Count() );

            access.CreateNewVersion( page );
            Assert.AreEqual( 3, GetVersions( pageName ).Count() );
        }

        [Test]
        public void CreateVersionShouldCopyThePage()
        {
            var access = new FlatFilePageHistoryAccess( mySampleRepository );
            var pageName = PageName.Create( "test" );
            var page = CreateOrUpdateTestPage( pageName, "one" );

            access.CreateNewVersion( page );
            Assert_ContentOfLastVersionEquals( pageName, "one" );

            page = CreateOrUpdateTestPage( pageName, "one", "two" );

            access.CreateNewVersion( page );
            Assert_ContentOfLastVersionEquals( pageName, "one", "two" );
        }

        [Test]
        public void CreateVersionForPageNameWithPath()
        {
            var access = new FlatFilePageHistoryAccess( mySampleRepository );
            var pageName = PageName.CreateFromPath( "/OpenSource/FreeBsd" );
            var page = CreateOrUpdateTestPage( pageName, "one" );

            access.CreateNewVersion( page );
            Assert_ContentOfLastVersionEquals( pageName, "one" );

            page = CreateOrUpdateTestPage( pageName, "one", "two" );

            access.CreateNewVersion( page );
            Assert_ContentOfLastVersionEquals( pageName, "one", "two" );
        }

        [Test]
        public void VersionsShouldBeRotatedIfLimitReached()
        {
            var access = new FlatFilePageHistoryAccess( mySampleRepository );

            var pageName = PageName.Create( "test" );
            var content = new List<string>();

            int numVersions = FlatFilePageHistoryAccess.MaxVersions + 3;
            numVersions.Times( i =>
                {
                    content = new List<string>();
                    i.Times( j => content.Add( "line-" + j ) );

                    var page = CreateOrUpdateTestPage( pageName, content.ToArray() );
                    access.CreateNewVersion( page );
                } );

            Assert.AreEqual( FlatFilePageHistoryAccess.MaxVersions, GetVersions( pageName ).Count() );

            Assert_ContentOfLastVersionEquals( pageName, content.ToArray() );
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

        private FlatFilePageDescriptor CreateOrUpdateTestPage( PageName name, params string[] content )
        {
            var file = myFileSystem.GetTempFile();
            file.WriteAll( content );

            return new FlatFilePageDescriptor( name, file );
        }

        private void Assert_ContentOfLastVersionEquals( PageName pageName, params string[] lines )
        {
            var content = GetContentOfLastVersion( pageName );

            CollectionAssert.AreEqual( lines, content );
        }

        private string[] GetContentOfLastVersion( PageName pageName )
        {
            var lastVersion = GetVersions( pageName )
                .OrderByDescending( file => file )
                .First();

            return lastVersion.ReadAllLines( );
        }

        private IEnumerable<IFile> GetVersions( PageName pageName )
        {
            var pathElements = pageName.Namespace.Elements.ToList();
            pathElements.Add( FlatFilePageHistoryAccess.HistoryDirectoryName );
            
            var historyDir = mySampleRepository.Directory(Path.Combine( pathElements.ToArray()  ) );

            return historyDir.GetFiles( "*." + FlatFilePageHistoryAccess.BackupFileExtension );
        }
    }
}
