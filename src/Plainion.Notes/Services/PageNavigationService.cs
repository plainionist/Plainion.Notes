using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace Plainion.Notes.Services
{
    [Export( typeof( PageNavigationService ) )]
    public class PageNavigationService
    {
        private IRegionManager myRegionManager;
        private PageName myCurrentPage;

        [ImportingConstructor]
        public PageNavigationService( IRegionManager regionManager )
        {
            myRegionManager = regionManager;

            var region = regionManager.Regions[ CompositionNames.PageRegion ];
            region.NavigationService.NavigationFailed += OnNavigationFailed;
            region.NavigationService.Navigated += OnNavigationCompleted;
        }

        private void OnNavigationCompleted( object sender, RegionNavigationEventArgs e )
        {
            CurrentPage = new PageNavigationParameters( e.NavigationContext.Parameters ).PageName;
        }

        public event EventHandler CurrentPageChanged;

        public PageName CurrentPage
        {
            get { return myCurrentPage; }
            private set
            {
                if( myCurrentPage == value )
                {
                    return;
                }

                myCurrentPage = value;

                if( CurrentPageChanged != null )
                {
                    CurrentPageChanged( this, EventArgs.Empty );
                }
            }
        }

        private void OnNavigationFailed( object sender, RegionNavigationFailedEventArgs e )
        {
            throw new InvalidOperationException( "Navigation failed", e.Error );
        }

        public void NavigateToRead( PageName page )
        {
            var args = new PageNavigationParameters();
            args.PageName = page;

            myRegionManager.RequestNavigate( CompositionNames.PageRegion, new Uri( CompositionNames.PageReadView, UriKind.Relative ), args.Parameters );
        }

        public void NavigateToEdit( PageName page )
        {
            var args = new PageNavigationParameters();
            args.PageName = page;
            args.CreateNew = false;

            myRegionManager.RequestNavigate( CompositionNames.PageRegion, new Uri( CompositionNames.PageEditView, UriKind.Relative ), args.Parameters );
        }

        internal void NavigateToCreate( PageName page )
        {
            var args = new PageNavigationParameters();
            args.PageName = page;
            args.CreateNew = true;

            myRegionManager.RequestNavigate( CompositionNames.PageRegion, new Uri( CompositionNames.PageEditView, UriKind.Relative ), args.Parameters );
        }
    }
}
