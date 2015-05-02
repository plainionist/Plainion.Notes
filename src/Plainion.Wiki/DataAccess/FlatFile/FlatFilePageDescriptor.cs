using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Plainion.IO;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess.FlatFile
{
    /// <summary>
    /// Implementation of a <see cref="IPageDescriptor"/> for a flat file.
    /// Implements caching of the file content based on LastModified time of the page file.
    /// </summary>
    public class FlatFilePageDescriptor : IPageDescriptor
    {
        private string[] myContent;
        private DateTime myLastUpdated;

        /// <summary/>
        public FlatFilePageDescriptor( PageName name, IFile file )
        {
            if ( name == null )
            {
                throw new ArgumentNullException( "name" );
            }
            if ( file == null )
            {
                throw new ArgumentNullException( "file" );
            }

            Name = name;
            File = file;
            myLastUpdated = DateTime.MinValue;
        }

        /// <summary/>
        public PageName Name
        {
            get;
            private set;
        }

        /// <summary/>
        public IFile File
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the content of the page.
        /// Throws an exception if the file has been removed in the meantime.
        /// </summary>
        public string[] GetContent()
        {
            if ( !File.Exists )
            {
                throw new Exception( "Invalid page descriptor. Page source no longer valid." );
            }

            if ( ContentNeedsUpdate )
            {
                myContent = ReadPageContent().ToArray();
            }

            return myContent;
        }

        private bool ContentNeedsUpdate
        {
            get
            {
                if ( myContent == null )
                {
                    return true;
                }

                if ( File.LastWriteTime.Ticks > myLastUpdated.Ticks )
                {
                    return true;
                }

                return false;
            }
        }

        private IList<string> ReadPageContent()
        {
            myLastUpdated = DateTime.Now;

            // wait 1 tick so that very fast update after this read
            // will not result in broken cache.
            // DateTime is not precise enough so we have to wait more than 16ms here
            // http://www.codeproject.com/KB/cs/DateTimePrecise.aspx?display=Print
            Thread.Sleep( 17 );

            var content = new List<string>();
            using ( var reader = File.CreateReader( FlatFilePageAccess.Encoding ) )
            {
                while ( reader.Peek() != -1 )
                {
                    content.Add( reader.ReadLine() );
                }
            }

            return content;
        }

        /// <summary/>
        public bool Matches( string searchText )
        {
            return searchText == null ||
                Name.Name.Contains( searchText, StringComparison.OrdinalIgnoreCase ) ||
                GetContent().Any( line => line.Contains( searchText, StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
