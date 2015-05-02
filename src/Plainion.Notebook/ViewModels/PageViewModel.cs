using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using Awesomium.Core;
using Plainion.Notebook.Events;
using Plainion.Notebook.Model;
using Plainion.Notebook.Services;
using Plainion.Notebook.Views;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion.AppFw.Wpf.Services;

namespace Plainion.Notebook.ViewModels
{
    [Export( typeof( PageViewModel ) )]
    class PageViewModel : PaneViewModel, IViewConnector
    {
        private Uri myUri;
        private WikiService myWikiService;
        private Uri myTargetUri;

        [ImportingConstructor]
        public PageViewModel( IEventAggregator eventAggregator, WikiService wikiService, IProjectService<Project> projectService,
            IPageNavigation navigation, DisplayUrlRequest request )
        {
            myWikiService = wikiService;

            IsSourceView = request.IsSourceView;

            NavigationSettings = new BrowserNavigationSettings();
            NavigationSettings.ShowNavigationBar = !IsSourceView;
            NavigationSettings.HomeUrl=myWikiService.HomePageUri;

            CloseCommand = new DelegateCommand( () => Close( eventAggregator ) );
            ViewPageSourceCommand = new DelegateCommand( () => navigation.OpenPageSourceView( Uri ) );
            OpenLinkInNewTabCommand = new DelegateCommand( () => navigation.OpenInNewTab( TargetUri ), () => TargetUri != null && !TargetUri.IsBlank() );
            OpenLinkInExternalBrowserCommand = new DelegateCommand( () => navigation.OpenInExternalBrowser( TargetUri ),
                () => TargetUri != null && !TargetUri.IsBlank() && myWikiService.IsExternalLink( TargetUri ) );
            OpenNewWindowRequestedCommand = new DelegateCommand<Uri>( url => navigation.OpenInNewTab( url ), url => url != null && !url.IsBlank() );

            BrowserPreferences = new WebPreferences
            {
                ShrinkStandaloneImagesToFit = false,
                SmoothScrolling = true
            };
            BrowserSession = Path.Combine( projectService.Project.DbFolder, ".Session" );

            // keep as last - activates the viewmodel indirectly
            Uri = request.TargetUri;
        }

        public DelegateCommand ViewPageSourceCommand { get; private set; }

        public DelegateCommand OpenLinkInNewTabCommand { get; private set; }

        public DelegateCommand OpenLinkInExternalBrowserCommand { get; private set; }

        public ICommand OpenNewWindowRequestedCommand { get; private set; }

        public WebPreferences BrowserPreferences { get; private set; }

        public string BrowserSession { get; private set; }

        public Uri Uri
        {
            get { return myUri; }
            set
            {
                if( SetProperty( ref myUri, value ) )
                {
                    var title = myWikiService.GetPageNameFromUri( myUri );
                    if( title.StartsWith( "/" ) )
                    {
                        title = title.Substring( 1 );
                    }
                    Title = title;

                    ContentId = myUri != null ? myWikiService.GetPageNameFromUri( myUri ) : null;

                    NavigationSettings.ShowSource = myWikiService.IsExternalLink( myUri );

                    ViewPageSourceCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Uri TargetUri
        {
            get { return myTargetUri; }
            set
            {
                if( SetProperty( ref myTargetUri, value ) )
                {
                    OpenLinkInNewTabCommand.RaiseCanExecuteChanged();
                    OpenLinkInExternalBrowserCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public BrowserNavigationSettings NavigationSettings
        {
            get;
            private set;
        }

        public bool IsSourceView
        {
            get;
            private set;
        }

        /// <summary>
        /// We explicitly maintain a separate property here for binding to browser title to be able to ignore
        /// title changed notifications with empty title to avoid flickering in tab.
        /// </summary>
        public string BrowserTitle
        {
            set
            {
                if( string.IsNullOrEmpty( value ) )
                {
                    return;
                }

                Title = value;
            }
        }

        public ICommand CloseCommand
        {
            get;
            private set;
        }

        private void Close( IEventAggregator eventAggregator )
        {
            eventAggregator.GetEvent<PageClosedEvent>().Publish( this );
        }

        public IDisposable View
        {
            get;
            set;
        }

        internal void Dispose()
        {
            if( View != null )
            {
                View.Dispose();
                View = null;
            }
        }
    }
}
