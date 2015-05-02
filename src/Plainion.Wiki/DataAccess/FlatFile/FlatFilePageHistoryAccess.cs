using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Plainion.IO;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess.FlatFile
{
    /// <summary>
    /// Implements <see cref="IPageHistoryAccess"/> with flat files.
    /// </summary>
    [Export( typeof( IPageHistoryAccess ) )]
    public class FlatFilePageHistoryAccess : IPageHistoryAccess
    {
        /// <summary>
        /// Max number of versions simulatiously supported. More versions will be rotated.
        /// </summary>
        public static readonly int MaxVersions = 5;

        /// <summary/>
        public static readonly string BackupFileExtension = "bwh";

        /// <summary/>
        public static readonly string HistoryDirectoryName = ".history";

        [ImportingConstructor]
        public FlatFilePageHistoryAccess( [Import( CompositionContractNames.HistoryRoot )] IDirectory historyRoot )
        {
            if ( historyRoot == null )
            {
                throw new ArgumentNullException( "historyRoot" );
            }

            HistoryRoot = historyRoot;
        }

        /// <summary>
        /// Directory containing the histories of the pages.
        /// </summary>
        public IDirectory HistoryRoot
        {
            get;
            private set;
        }

        /// <summary/>
        public void CreateNewVersion( IPageDescriptor pageDescriptor )
        {
            if ( pageDescriptor == null )
            {
                throw new ArgumentNullException( "pageDescriptor" );
            }

            RotateHistory( pageDescriptor.Name );

            var backupFile = CreateBackupFileFromPageName( pageDescriptor.Name );
            backupFile.WriteAll( ( (FlatFilePageDescriptor)pageDescriptor ).GetContent() );
        }

        private void RotateHistory( PageName pageName )
        {
            var allVersions = GetHistoryDirectory( pageName ).GetFiles( pageName.Name + ".*." + BackupFileExtension )
                .OrderBy( file => file )
                .ToList();

            while ( allVersions.Count >= MaxVersions )
            {
                allVersions.First().Delete();
                allVersions.RemoveAt( 0 );
            }
        }

        private IFile CreateBackupFileFromPageName( PageName pageName )
        {
            // Wait exactly one tick so that we never return the same name twice
            Thread.Sleep( 1 );

            var file = string.Format( "{0}.{1}.{2}", pageName.Name, Stopwatch.GetTimestamp(), BackupFileExtension );
            return GetHistoryDirectory( pageName ).File( file );
        }

        private IDirectory GetHistoryDirectory( PageName pageName )
        {
            var pathElements = pageName.Namespace.Elements.ToList();
            pathElements.Add( HistoryDirectoryName );

            var historyDir = HistoryRoot.Directory( Path.Combine( pathElements.ToArray() ) );
            historyDir.Create();

            return historyDir;
        }
    }
}
