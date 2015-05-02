using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Plainion.Notes.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Plainion.Notes.ViewModels
{
    [Export( typeof( PageNavigationViewModel ) )]
    public class PageNavigationViewModel : BindableBase
    {
        private WikiService myWikiService;
        private PageNavigationService myNavigationService;
        private string myPagePosition;
        private string myPageTitle;

        [ImportingConstructor]
        public PageNavigationViewModel( WikiService wikiService, PageNavigationService navigationService )
        {
            myWikiService = wikiService;
            myNavigationService = navigationService;

            NavigateBack = new DelegateCommand( OnBack, CanBack );
            NavigateForward = new DelegateCommand( OnForward, CanForward );
            NavigateHome = new DelegateCommand( OnHome );

            myWikiService.Pages.CollectionChanged += OnPagesChanged;
            myNavigationService.CurrentPageChanged += OnCurrentPageChanged;

            Evaluate();
        }

        private void Evaluate()
        {
            NavigateBack.RaiseCanExecuteChanged();
            NavigateForward.RaiseCanExecuteChanged();
            
            if( myNavigationService.CurrentPage == null )
            {
                PagePosition = "0/0";
                return;
            }

            var pageCount = myWikiService.Pages.Count;
            var pageIdx = myWikiService.Pages.IndexOf( myNavigationService.CurrentPage );

            PagePosition = string.Format( "{0}/{1}", pageIdx + 1, pageCount );

            PageTitle = myNavigationService.CurrentPage.Name;
        }

        private void OnCurrentPageChanged( object sender, EventArgs e )
        {
            Evaluate();
        }

        private void OnPagesChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            Evaluate();
        }

        private bool CanBack()
        {
            return myWikiService.Pages.IndexOf( myNavigationService.CurrentPage ) > 0;
        }

        private void OnBack()
        {
            var targetPage = myWikiService.Pages[ myWikiService.Pages.IndexOf( myNavigationService.CurrentPage ) - 1 ];
            myNavigationService.NavigateToRead( targetPage );
        }

        private bool CanForward()
        {
            return myWikiService.Pages.IndexOf( myNavigationService.CurrentPage ) < myWikiService.Pages.Count - 1;
        }

        private void OnForward()
        {
            var targetPage = myWikiService.Pages[ myWikiService.Pages.IndexOf( myNavigationService.CurrentPage ) + 1 ];
            myNavigationService.NavigateToRead( targetPage );
        }

        private void OnHome()
        {
            myNavigationService.NavigateToRead( myWikiService.HomePage );
        }

        public DelegateCommand NavigateBack { get; private set; }
        public DelegateCommand NavigateForward { get; private set; }
        public ICommand NavigateHome { get; private set; }

        public string PagePosition
        {
            get { return myPagePosition; }
            set { SetProperty( ref myPagePosition, value ); }
        }

        public string PageTitle
        {
            get { return myPageTitle; }
            set { SetProperty( ref myPageTitle, value ); }
        }
    }
}
