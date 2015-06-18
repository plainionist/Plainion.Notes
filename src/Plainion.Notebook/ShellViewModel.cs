using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Plainion.Notebook.Events;
using Plainion.Notebook.Model;
using Plainion.Notebook.Services;
using Plainion.Notebook.ViewModels;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion.AppFw.Wpf.Services;
using Plainion.AppFw.Wpf.ViewModels;
using Plainion.Prism.Events;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Web;

namespace Plainion.Notebook
{
    [Export(typeof(IPageNavigation))]
    [Export(typeof(ShellViewModel))]
    class ShellViewModel : BindableBase, IPageNavigation
    {
        private const string AppName = "Plainion.Notebook";

        private IEventAggregator myEventAggregator;
        private ObservableCollection<PageViewModel> myPages;
        private ObservableCollection<ToolViewModel> myTools;
        private PageViewModel myActivePage;
        private PageViewModelFactory myPageViewModelFactory;
        private IProjectService<Project> myProjectService;
        private IPersistenceService<Project> myPersistenceService;
        private NavigationViewModel myNavigation;
        private SearchResultsViewModel mySearchResults;

        [Import]
        private Lazy<Serializer> Serializer { get; set; }

        [Import]
        private WikiService WikiService { get; set; }

        [Import(AllowRecomposition = true)]
        private Lazy<DockingManager> DockingManager { get; set; }

        [ImportingConstructor]
        public ShellViewModel(IProjectService<Project> projectService, IEventAggregator eventAggregator, IPersistenceService<Project> persistenceService,
            PageViewModelFactory pageViewModelFactory)
        {
            myProjectService = projectService;
            myPersistenceService = persistenceService;
            myEventAggregator = eventAggregator;
            myPageViewModelFactory = pageViewModelFactory;

            myProjectService.ProjectChanging += OnProjectChanging;
            myProjectService.ProjectChanged += OnProjectChanged;

            myPages = new ObservableCollection<PageViewModel>();
            Pages = new ReadOnlyObservableCollection<PageViewModel>(myPages);

            myTools = new ObservableCollection<ToolViewModel>();
            Tools = new ReadOnlyObservableCollection<ToolViewModel>(myTools);

            myTools.Add(DummyTool.Instance);

            PrintCommand = new DelegateCommand(OnPrint, () => IsProjectLoaded);

            myEventAggregator.GetEvent<ApplicationReadyEvent>().Subscribe(x => OnApplicationReady());
            myEventAggregator.GetEvent<ApplicationShutdownEvent>().Subscribe(x => OnShutdown());
            myEventAggregator.GetEvent<PageClosedEvent>().Subscribe(OnPageClosed);
        }

        [Import]
        public ProjectLifecycleViewModel<Project> ProjectLifecycleViewModel { get; set; }

        [Import]
        public TitleViewModel<Project> TitleViewModel { get; set; }

        public DelegateCommand PrintCommand { get; private set; }

        private void OnPrint()
        {
            var uri = WikiService.GetUriFromPath(HttpUtility.UrlEncode(WikiService.GetMetadata().PrintPreviewPageName.FullName + "?action=AllInOne"));
            OpenInNewTab(uri);
            ActivePage = myPages.Last();
        }

        private void OnPageClosed(PageViewModel pageViewModel)
        {
            if (ActivePage == pageViewModel)
            {
                ActivePage = null;
            }

            pageViewModel.Dispose();

            myPages.Remove(pageViewModel);
        }

        // http://avalondock.codeplex.com/discussions/472420
        private class DummyTool : ToolViewModel
        {
            private DummyTool()
                : base("dummy")
            {
                IsVisible = false;
            }

            public static DummyTool Instance = new DummyTool();
        }

        private void OnShutdown()
        {
            if (myProjectService.Project != null)
            {
                Serializer.Value.SaveLayout(myProjectService.Project);
            }
        }

        // we need to cleanup the WebViews before the new deamon gets started
        private void OnProjectChanging(object sender, EventArgs e)
        {
            SearchResults = null;
            Navigation = null;
            ActivePage = null;

            foreach (var page in myPages)
            {
                page.Dispose();
            }

            myPages.Clear();

            if (!myTools.Contains(DummyTool.Instance))
            {
                myTools.Add(DummyTool.Instance);
            }

            foreach (var tool in myTools.Where(t => t != DummyTool.Instance).ToList())
            {
                var disposable = tool as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                myTools.Remove(tool);
            }
        }

        private void OnProjectChanged(object sender, EventArgs e)
        {
            var layoutWasDeserialized = Serializer.Value.LoadLayout(myProjectService.Project);

            if (!myPages.Any())
            {
                myPages.Add(myPageViewModelFactory.Create(new DisplayUrlRequest(WikiService.HomePageUri)));
            }

            ActivePage = myPages.Last();

            if (!myTools.Where(t => t != DummyTool.Instance).Any())
            {
                Navigation = new NavigationViewModel(WikiService, myProjectService, this);
                myTools.Add(Navigation);

                SearchResults = new SearchResultsViewModel(myProjectService, this);
                myTools.Add(SearchResults);
            }
            else
            {
                Navigation = myTools.OfType<NavigationViewModel>().Single();
                SearchResults = myTools.OfType<SearchResultsViewModel>().Single();
            }

            myTools.Remove(DummyTool.Instance);

            InitTool(Navigation, layoutWasDeserialized);
            InitTool(SearchResults, layoutWasDeserialized);

            OnPropertyChanged("IsProjectLoaded");
            PrintCommand.RaiseCanExecuteChanged();
        }

        private void InitTool(ToolViewModel tool, bool layoutWasDeserialized)
        {
            tool.IsSelected = false;
            tool.IsActive = false;
            tool.IsVisible = true;

            var y = DockingManager.Value.Layout.Descendents().OfType<LayoutAnchorable>().ToList();
            var layoutItem = DockingManager.Value.Layout.Descendents().OfType<LayoutAnchorable>()
                .Single(x => x.ContentId == tool.ContentId);

            if (!layoutWasDeserialized)
            {
                layoutItem.AutoHideMinHeight = 100;
                layoutItem.AutoHideMinWidth = 200;
            }

            if (layoutItem.IsAutoHidden)
            {
                // HACK: i found no other way to auto hide on startup without temporarily seeing the pane
                layoutItem.ToggleAutoHide();
                layoutItem.ToggleAutoHide();
            }
        }

        public bool IsProjectLoaded
        {
            get { return myProjectService.Project != null; }
        }

        public ReadOnlyObservableCollection<PageViewModel> Pages
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollection<ToolViewModel> Tools
        {
            get;
            private set;
        }

        public NavigationViewModel Navigation
        {
            get { return myNavigation; }
            private set { SetProperty(ref myNavigation, value); }
        }

        public SearchResultsViewModel SearchResults
        {
            get { return mySearchResults; }
            private set { SetProperty(ref mySearchResults, value); }
        }

        private void OnApplicationReady()
        {
            TitleViewModel.ApplicationName = AppName;

            ProjectLifecycleViewModel.ApplicationName = AppName;
            ProjectLifecycleViewModel.FileFilter = "Plainion Notebook Projects (*.bnt)|*.bnt";
            ProjectLifecycleViewModel.FileFilterIndex = 0;
            ProjectLifecycleViewModel.DefaultFileExtension = ".bnt";
            ProjectLifecycleViewModel.AutoSaveNewProject = true;

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                myProjectService.Project = myPersistenceService.Load(args[1]);
            }
        }

        public void OpenPageSourceView(Uri url)
        {
            var request = new DisplayUrlRequest(url);
            request.IsSourceView = true;

            var pageViewModel = myPageViewModelFactory.Create(request);

            myPages.Add(pageViewModel);

            ActivePage = pageViewModel;
        }

        public void OpenInNewTab(Uri url)
        {
            var pageViewModel = myPageViewModelFactory.Create(new DisplayUrlRequest(url));

            myPages.Add(pageViewModel);
        }

        public void OpenInExternalBrowser(Uri url)
        {
            Process.Start(url.ToString());
        }

        public void OpenInActiveTab(Uri url)
        {
            if (url.Query.Contains("action=search"))
            {
                SearchResults.Uri = url;

                SearchResults.IsVisible = true;
                SearchResults.IsActive = true;
                SearchResults.IsSelected = true;
            }
            else if (ActivePage != null)
            {
                ActivePage.Uri = url;
            }
            else
            {
                OpenInNewTab(url);
            }
        }

        public PageViewModel ActivePage
        {
            get { return myActivePage; }
            set { SetProperty(ref myActivePage, value); }
        }

        [Export]
        private void OnLayoutSerialization(object sender, LayoutSerializationCallbackEventArgs args)
        {
            if (args.Model.ContentId == NavigationViewModel.ToolContentId)
            {
                var navi = new NavigationViewModel(WikiService, myProjectService, this);
                myTools.Add(navi);
                args.Content = navi;
            }
            else if (args.Model.ContentId == SearchResultsViewModel.ToolContentId)
            {
                var searchResults = new SearchResultsViewModel(myProjectService, this);
                myTools.Add(searchResults);
                args.Content = searchResults;
            }
            else if (!string.IsNullOrWhiteSpace(args.Model.ContentId))
            {
                var request = new DisplayUrlRequest(WikiService.GetUriFromPath(args.Model.ContentId));
                var pageViewModel = myPageViewModelFactory.Create(request);

                myPages.Add(pageViewModel);

                args.Content = pageViewModel;
            }
        }
    }
}
