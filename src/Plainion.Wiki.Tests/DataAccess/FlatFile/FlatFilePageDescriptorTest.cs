using System;
using System.Threading;
using Plainion.IO;
using Plainion.IO.MemoryFS;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess.FlatFile;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.DataAccess.FlatFile
{
    [TestFixture]
    public class FlatFilePageDescriptorTest
    {
        private IFileSystem myFileSystem;

        [SetUp]
        public void SetUp()
        {
            myFileSystem = new FileSystemImpl();
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithoutName()
        {
            new FlatFilePageDescriptor( null, myFileSystem.File( "somefile" ) );
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateWithoutPath()
        {
            new FlatFilePageDescriptor( PageName.Create( "SomePage" ), null );
        }

        [Test]
        public void Create_WithInvlidPath_ShouldNotThrow()
        {
            var pageName = PageName.Create( "SomePage" );

            // exception is only thrown when asking for the content
            var descriptor = new FlatFilePageDescriptor( pageName, myFileSystem.File( @"c:\SomeFile.txt" ) );

            Assert.AreEqual( pageName, descriptor.Name );
            Assert.AreEqual( @"c:\SomeFile.txt", descriptor.File.Path );
        }

        [Test]
        public void Create()
        {
            var file = myFileSystem.GetTempFile();
            file.Create();

            var pageName = PageName.Create( "SomePage" );
            var descriptor = new FlatFilePageDescriptor( pageName, file );

            Assert.AreEqual( pageName, descriptor.Name );
            Assert.AreEqual( file, descriptor.File );
        }

        [Test]
        public void GetContent()
        {
            var file = myFileSystem.GetTempFile();
            file.WriteAll( "some text" );

            var descriptor = new FlatFilePageDescriptor( PageName.Create( "SomePage" ), file );

            var lines = descriptor.GetContent();

            Assert.AreEqual( 1, lines.Length );
            Assert.AreEqual( "some text", lines[ 0 ] );
        }

        [Test]
        public void ValidateCachingOfContent()
        {
            var file = myFileSystem.GetTempFile();
            file.WriteAll( "some text" );

            var descriptor = new FlatFilePageDescriptor( PageName.Create( "SomePage" ), file );

            descriptor.GetContent();

            var lastAccess = file.LastAccessTime;

            // this call should be against the cache
            descriptor.GetContent();

            Assert.AreEqual( lastAccess.Ticks, file.LastAccessTime.Ticks );

            // touch the file
            file.WriteAll( "some other text" );

            // so now this call should be against the real file
            descriptor.GetContent();

            Assert.AreNotEqual( lastAccess.Ticks, file.LastAccessTime.Ticks );
        }

        [Test]
        [ExpectedException( ExpectedException = typeof( Exception ), ExpectedMessage = "Invalid page descriptor", MatchType = MessageMatch.Contains )]
        public void InvalidPageDescriptor()
        {
            var file = myFileSystem.GetTempFile();
            file.WriteAll( "some text" );

            var descriptor = new FlatFilePageDescriptor( PageName.Create( "SomePage" ), file );

            descriptor.GetContent();

            Thread.Sleep( 1 );
            file.Delete();

            descriptor.GetContent();
        }
    }
}
