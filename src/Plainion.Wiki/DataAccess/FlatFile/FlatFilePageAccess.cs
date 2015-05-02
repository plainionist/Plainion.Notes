using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Plainion.IO;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess.FlatFile
{
    /// <summary>
    /// Implements <see cref="IPageAccess"/> for flat filesystem pages.
    /// </summary>
    [Export( typeof( IPageAccess ) )]
    public class FlatFilePageAccess : IPageAccess
    {
        private IList<IPageDescriptor> myPages;
        private IPageHistoryAccess myHistoryAccess;

        /// <summary>
        /// Default encoding for files.
        /// </summary>
        public static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary/>
        public static readonly string FileExtension = "bwi";

        [ImportingConstructor]
        public FlatFilePageAccess( [Import( CompositionContractNames.PageRoot )] IDirectory pageRoot, IPageHistoryAccess historyAccess )
        {
            if ( pageRoot == null )
            {
                throw new ArgumentNullException( "pageRoot" );
            }
            if ( !pageRoot.Exists )
            {
                throw new FileNotFoundException( pageRoot.Path );
            }

            PageRepository = pageRoot;
            myHistoryAccess = historyAccess;

            myPages = new List<IPageDescriptor>();
            LoadAllPageDescriptors();
        }

        /// <summary>
        /// Directory containing raw pages.
        /// </summary>
        public IDirectory PageRepository
        {
            get;
            private set;
        }

        private void LoadAllPageDescriptors()
        {
            myPages = AllPageFiles
                .Select( file => GetOrCreateDescriptor( file ) )
                .ToList();
        }

        private IEnumerable<IFile> AllPageFiles
        {
            get
            {
                return PageRepository.GetFiles( "*." + FileExtension, SearchOption.AllDirectories );
            }
        }

        private IPageDescriptor GetOrCreateDescriptor( IFile file )
        {
            var pageName = GetPageNameFromFile( file );

            var page = myPages.SingleOrDefault( p => p.Name == pageName );
            if ( page == null )
            {
                page = new FlatFilePageDescriptor( pageName, file );
            }

            return page;
        }

        private PageName GetPageNameFromFile( IFile file )
        {
            var fullPageName = file.Path.Substring( PageRepository.Path.Length, file.Path.Length - PageRepository.Path.Length );

            while ( fullPageName[ 0 ] == '\\' )
            {
                fullPageName = fullPageName.Substring( 1 );
            }

            fullPageName = fullPageName.Substring( 0, fullPageName.Length - FileExtension.Length - 1 );
            fullPageName = fullPageName.Replace( '\\', '/' );
            return PageName.CreateFromPath( fullPageName ); ;
        }

        /// <summary/>
        public IEnumerable<IPageDescriptor> Pages
        {
            get { return myPages; }
        }

        /// <summary/>
        public IPageDescriptor Find( PageName pageName )
        {
            return Pages.FirstOrDefault( page => page.Name == pageName );
        }

        /// <summary/>
        public void Create( IPageDescriptor pageDescriptor )
        {
            CreateOrUpdate( pageDescriptor );
        }

        /// <summary/>
        public void Update( IPageDescriptor pageDescriptor )
        {
            CreateOrUpdate( pageDescriptor );
        }

        private void CreateOrUpdate( IPageDescriptor pageDescriptor )
        {
            var existingPage = Find( pageDescriptor.Name );

            if ( existingPage != null )
            {
                myHistoryAccess.CreateNewVersion( existingPage );
            }
            else
            {
                existingPage = GetOrCreateDescriptor( GetFileFromPageName( pageDescriptor ) );
                myPages.Add( existingPage );
            }

            WritePage( ( (FlatFilePageDescriptor)existingPage ).File, pageDescriptor.GetContent() );
        }

        private void WritePage( IFile file, string[] content )
        {
            var pageDir = file.Parent;
            if ( !pageDir.Exists )
            {
                pageDir.Create();
            }

            using ( var writer = file.CreateWriter( Encoding ) )
            {
                foreach ( var line in content )
                {
                    writer.WriteLine( line );
                }
            }
        }

        private IFile GetFileFromPageName( IPageDescriptor pageDescriptor )
        {
            return GetFileFromPageName( pageDescriptor.Name );
        }

        private IFile GetFileFromPageName( PageName pageName )
        {
            var nonRootedFullName = pageName.FullName.Substring( 1 );
            return PageRepository.File( nonRootedFullName + "." + FileExtension );
        }

        /// <summary/>
        public void Delete( IPageDescriptor pageDescriptor )
        {
            var existingPage = Find( pageDescriptor.Name );
            if ( existingPage == null )
            {
                throw new InvalidOperationException( "No such page: " + pageDescriptor.Name );
            }

            myHistoryAccess.CreateNewVersion( existingPage );

            var pageFile = GetFileFromPageName( pageDescriptor );

            pageFile.Delete();
            myPages.Remove( existingPage );
        }

        /// <summary>
        /// Does not update history
        /// </summary>
        public void Move( PageName pageName, PageNamespace newNamespace )
        {
            var existingPage = Find( pageName );
            if ( existingPage == null )
            {
                throw new InvalidOperationException( "No such page: " + pageName );
            }

            var pageFile = GetFileFromPageName( pageName );

            var newPageFile = pageFile.Move( GetDirectoryFromNamespace( newNamespace ) );

            myPages.Remove( existingPage );

            var newPage = GetOrCreateDescriptor( newPageFile );
            myPages.Add( newPage );
        }

        private IDirectory GetDirectoryFromNamespace( PageNamespace newNamespace )
        {
            var nonRootedFullName = newNamespace.AsPath.Substring( 1 );
            return PageRepository.Directory( nonRootedFullName );
        }
    }
}
