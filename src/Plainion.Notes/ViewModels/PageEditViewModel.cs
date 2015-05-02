using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Plainion.Notes.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Plainion.Windows.Controls;

namespace Plainion.Notes.ViewModels
{
    [Export]
    public class PageEditViewModel : PageViewModelBase
    {
        private string myPageText;

        [ImportingConstructor]
        public PageEditViewModel( WikiService wikiService, PageNavigationService navigationService )
            : base( wikiService, navigationService )
        {
            SaveCommand = new DelegateCommand( OnSave );
            DeleteCommand = new DelegateCommand( OnDelete, () => PageName != WikiService.HomePage );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        protected override void OnPageNameChanged()
        {
            DeleteCommand.RaiseCanExecuteChanged();
        }

        private void OnSave()
        {
            TextBoxBinding.ForceSourceUpdate();
            
            WikiService.Save( PageName, PageText );
            NavigationService.NavigateToRead( PageName );
        }

        private void OnDelete()
        {
            WikiService.Delete( PageName );
            NavigationService.NavigateToRead( WikiService.HomePage );
        }

        private void OnCancel()
        {
            NavigationService.NavigateToRead( PageName );
        }

        public ICommand SaveCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public string PageText
        {
            get { return myPageText; }
            set { SetProperty( ref myPageText, value ); }
        }

        protected override void OnNavigatedToCompleted( PageNavigationParameters args )
        {
            if( args.CreateNew )
            {
                PageText = "Enter page content here";
            }
            else
            {
                PageText = string.Join( Environment.NewLine, WikiService.GetPageText( PageName ) );
            }
        }
    }
}
