using System.ComponentModel.Composition;
using Plainion.Notes.Services;
using Plainion.Wiki.AST;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace Plainion.Notes.ViewModels
{
    public abstract class PageViewModelBase : BindableBase, INavigationAware
    {
        private PageName myPageName;

        [ImportingConstructor]
        public PageViewModelBase( WikiService wikiService, PageNavigationService navigationService )
        {
            WikiService = wikiService;
            NavigationService = navigationService;
        }

        public PageName PageName
        {
            get { return myPageName; }
            private set
            {
                if( SetProperty( ref myPageName, value ) )
                {
                    OnPageNameChanged();
                }
            }
        }

        protected virtual void OnPageNameChanged()
        {
        }

        protected WikiService WikiService { get; private set; }

        protected PageNavigationService NavigationService { get; private set; }

        public bool IsNavigationTarget( NavigationContext navigationContext )
        {
            return true;
        }

        public void OnNavigatedFrom( NavigationContext navigationContext )
        {
        }

        public void OnNavigatedTo( NavigationContext navigationContext )
        {
            var args = new PageNavigationParameters( navigationContext.Parameters );
            PageName = args.PageName;
            OnNavigatedToCompleted( args );
        }

        protected abstract void OnNavigatedToCompleted( PageNavigationParameters args );
    }
}
