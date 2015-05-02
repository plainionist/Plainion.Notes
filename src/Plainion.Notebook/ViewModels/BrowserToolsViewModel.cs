using System;
using System.IO;
using System.Windows.Input;
using Awesomium.Core;
using Plainion.Notebook.Model;
using Plainion.Notebook.Views;
using Microsoft.Practices.Prism.Commands;
using Plainion.AppFw.Wpf.Services;

namespace Plainion.Notebook.ViewModels
{
    class BrowserToolsViewModel : ToolViewModel, IViewConnector
    {
        private Uri myUri;
        private Uri myTargetUri;

        protected BrowserToolsViewModel( IProjectService<Project> projectService, IPageNavigation navigation, string name, string contentId )
            : base( name )
        {
            ContentId = contentId;

            NavigationSettings = new BrowserNavigationSettings();
            NavigationSettings.ShowSource = false;
            NavigationSettings.BrowseBackEnabled = false;
            NavigationSettings.BrowseForwardEnabled = false;
            NavigationSettings.BrowseHomeEnabled = false;

            IsVisible = false;

            OpenNewWindowRequestedCommand = new DelegateCommand<Uri>( url => navigation.OpenInActiveTab( url ), url => url != null && !url.IsBlank() );
            OpenLinkInActiveTabCommand = new DelegateCommand( () => navigation.OpenInActiveTab( TargetUri ), () => TargetUri != null && !TargetUri.IsBlank() );
            OpenLinkInNewTabCommand = new DelegateCommand( () => navigation.OpenInNewTab( TargetUri ), () => TargetUri != null && !TargetUri.IsBlank() );

            BrowserPreferences = new WebPreferences
            {
                ShrinkStandaloneImagesToFit = false,
                SmoothScrolling = true
            };
            BrowserSession = Path.Combine( projectService.Project.DbFolder, ".Session" );
        }

        public ICommand OpenNewWindowRequestedCommand { get; private set; }

        public DelegateCommand OpenLinkInActiveTabCommand { get; private set; }

        public DelegateCommand OpenLinkInNewTabCommand { get; private set; }

        public BrowserNavigationSettings NavigationSettings
        {
            get;
            private set;
        }

        public WebPreferences BrowserPreferences { get; private set; }

        public string BrowserSession { get; private set; }

        public Uri Uri
        {
            get { return myUri; }
            set { SetProperty( ref myUri, value ); }
        }

        public Uri TargetUri
        {
            get { return myTargetUri; }
            set
            {
                if( SetProperty( ref myTargetUri, value ) )
                {
                    OpenLinkInActiveTabCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public IDisposable View
        {
            get;
            set;
        }

        public void Dispose()
        {
            if( View != null )
            {
                View.Dispose();
                View = null;
            }
        }
    }
}
