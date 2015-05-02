using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Parser;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// High-level access to parsed pages. Parses pages on demand and
    /// implements caching of parsed pages.
    /// </summary>
    [Export( typeof( PageRepository ) )]
    public class PageRepository
    {
        private IPageAccess myPageAccess;
        private PageCache myCache;
        private ParserPipeline myPageParser;
        private IPageTransformer myPageUpdatePreProcessor;

        [ImportingConstructor]
        public PageRepository( IPageAccess pageAccess, ParserPipeline parser )
        {
            if ( pageAccess == null )
            {
                throw new ArgumentNullException( "pageAccess" );
            }
            if ( parser == null )
            {
                throw new ArgumentNullException( "parser" );
            }

            myPageAccess = pageAccess;
            myPageParser = parser;

            LoadWikiWords();

            myCache = new PageCache();
            // set default
            PageUpdatePreProcessor = null;
        }

        private void LoadWikiWords()
        {
            myPageParser.WikiWords = new WikiWords();

            myPageParser.WikiWords.Add( myPageAccess.Pages.Select( pd => pd.Name ) );
        }

        /// <summary>
        /// Allows preprocessing on pages before given to the <see cref="IPageAccess"/>.
        /// </summary>
        [Import( CompositionContractNames.PageUpdatePreProcessor, AllowDefault = true )]
        public IPageTransformer PageUpdatePreProcessor
        {
            get { return myPageUpdatePreProcessor; }
            set
            {
                myPageUpdatePreProcessor = value ?? new NullPageTransformer();
            }
        }

        /// <summary/>
        public IEnumerable<IPageDescriptor> Pages
        {
            get { return myPageAccess.Pages; }
        }

        /// <summary/>
        public IPageDescriptor Find( PageName pageName )
        {
            return myPageAccess.Find( pageName );
        }

        /// <summary/>
        public void Create( PageName pageName, IEnumerable<string> pageContent )
        {
            Create( new InMemoryPageDescriptor( pageName, pageContent ) );
        }

        /// <summary/>
        public void Create( IPageDescriptor pageDescriptor )
        {
            var descriptor = PageUpdatePreProcessor.Transform( pageDescriptor );
            myPageAccess.Create( descriptor );

            UpdateWikiWordsOnDemand( pageDescriptor.Name );
        }

        private void UpdateWikiWordsOnDemand( PageName pageName )
        {
            LoadWikiWords();

            myCache.Clear();
        }

        /// <summary/>
        public void Delete( IPageDescriptor pageDescriptor )
        {
            myPageAccess.Delete( pageDescriptor );
            myCache.Remove( pageDescriptor.Name );
            UpdateWikiWordsOnDemand( pageDescriptor.Name );
        }

        /// <summary/>
        public void Delete( PageName pageName )
        {
            var page = Find( pageName );
            if ( page != null )
            {
                Delete( page );
            }
        }

        /// <summary/>
        public void Update( PageName pageName, IEnumerable<string> pageContent )
        {
            Update( new InMemoryPageDescriptor( pageName, pageContent ) );
        }

        /// <summary/>
        public void Update( IPageDescriptor pageDescriptor )
        {
            var descriptor = PageUpdatePreProcessor.Transform( pageDescriptor );

            myPageAccess.Update( descriptor );

            myCache.Remove( descriptor.Name );
        }

        /// <summary/>
        public PageBody Get( PageName pageName )
        {
            var descriptor = Find( pageName );
            if ( descriptor == null )
            {
                return null;
            }

            return Get( descriptor );
        }

        /// <summary/>
        public PageBody Get( IPageDescriptor pageDescriptor )
        {
            if ( pageDescriptor == null )
            {
                throw new ArgumentNullException( "pageDescriptor" );
            }

            var page = myCache.FindByName( pageDescriptor.Name );
            if ( page != null )
            {
                return page;
            }

            //if ( Find( pageDescriptor.Name ) == null )
            //{
            //    // TODO: throw exception here?
            //    // page not known to PageAccess - deny parsing and caching it
            //    // even more difficult: we also get InMemory pages here like "PageNotFound"
            //    return null;
            //}

            page = myPageParser.Parse( pageDescriptor );
            myCache.Add( page );

            return page;
        }

        /// <summary>
        /// Moves the given page to the new namespace. References to this page will NOT be updated.
        /// </summary>
        public void Move( PageName pageName, PageNamespace newNamespace )
        {
            myPageAccess.Move( pageName, newNamespace );

            //UpdatePageReferences( pageName, PageName.Create( newNamespace, pageName.Name ) );

            myCache.Remove( pageName );

            UpdateWikiWordsOnDemand( pageName );
        }

        //private void UpdatePageReferences( PageName oldPageName, PageName newPageName )
        //{
        //    var modifiedPages = new List<PageBody>();

        //    foreach ( var page in Pages.Select( descriptor => Get( descriptor ) ) )
        //    {
        //        bool pageModified = UpdatePageReferences( page, oldPageName, newPageName );
        //        if ( pageModified )
        //        {
        //            modifiedPages.Add( page );
        //        }
        //    }

        //    UpdatePageContent( modifiedPages );
        //}

        //private bool UpdatePageReferences( PageBody page, PageName oldPageName, PageName newPageName )
        //{
        //    var finder = new AstFinder<Link>( link => link.IsLinkingPage( oldPageName ) );
        //    var pageReferences = finder.Where( page );

        //    foreach ( var pageRef in pageReferences )
        //    {
        //        pageRef.Parent.ReplaceChild( pageRef, new Link( newPageName ) );
        //    }

        //    return pageReferences.Any();
        //}

        //private void UpdatePageContent( IEnumerable<PageBody> pages )
        //{
        //    foreach ( var page in pages )
        //    {
        //        UpdatePageContent( page );
        //    }
        //}

        //private void UpdatePageContent( PageBody page )
        //{
        //    myCache.Remove( page.Name );

        //    // TODO: currently not possible due to lack of "AST-to-wiki lang" renderer
        //    // if we impl s.th. how to we make sure that there is a proper render action for each ast?
        //    // also for the ast provided by user of Wiki library?
        //    // in error case we would need to do a rollback otherwise ..
        //    throw new NotImplementedException();
        //}
    }
}
