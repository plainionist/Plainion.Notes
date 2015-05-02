using System;
using Microsoft.Practices.Prism.Mvvm;

namespace Plainion.Notebook.Views
{
    public class BrowserNavigationSettings : BindableBase
    {
        private bool myShowNavigationBar;
        private bool myShowSource;
        private Uri myHomeUrl;
        private bool myBrowseBackEnabled;
        private bool myBrowseForwardEnabled;
        private bool myBrowseHomeEnabled;
        private bool myRefreshEnabled;

        public BrowserNavigationSettings()
        {
            ShowNavigationBar = true;
            ShowSource = true;

            HomeUrl = new Uri( "about:blank" );

            BrowseBackEnabled = true;
            BrowseForwardEnabled = true;
            BrowseHomeEnabled = true;
            RefreshEnabled = true;
        }

        public bool ShowNavigationBar
        {
            get { return myShowNavigationBar; }
            set { SetProperty( ref myShowNavigationBar, value ); }
        }

        public bool ShowSource
        {
            get { return myShowSource; }
            set { SetProperty( ref myShowSource, value ); }
        }

        public Uri HomeUrl
        {
            get { return myHomeUrl; }
            set { SetProperty( ref myHomeUrl, value ); }
        }

        public bool BrowseBackEnabled
        {
            get { return myBrowseBackEnabled; }
            set { SetProperty( ref myBrowseBackEnabled, value ); }
        }

        public bool BrowseForwardEnabled
        {
            get { return myBrowseForwardEnabled; }
            set { SetProperty( ref myBrowseForwardEnabled, value ); }
        }

        public bool BrowseHomeEnabled
        {
            get { return myBrowseHomeEnabled; }
            set { SetProperty( ref myBrowseHomeEnabled, value ); }
        }

        public bool RefreshEnabled
        {
            get { return myRefreshEnabled; }
            set { SetProperty( ref myRefreshEnabled, value ); }
        }
    }
}
