using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Awesomium.Core;
using Awesomium.Windows.Controls;
using Plainion.Notebook.ViewModels;

namespace Plainion.Notebook.Views
{
    // http://wiki.awesomium.net/wpf/walkthrough-webcontrol.html
    public partial class Browser : UserControl, IDisposable
    {
        private DispatcherTimer myTimer;
        private bool myIsBeingDraging;

        public Browser()
        {
            InitializeComponent();

            CommandBindings.Add( new CommandBinding( NavigationCommands.BrowseHome, ( s, e ) => myWebControl.Source = NavigationSettings.HomeUrl, ( s, e ) => e.CanExecute = true ) );
            CommandBindings.Add( new CommandBinding( NavigationCommands.BrowseBack, ( s, e ) => myWebControl.GoBack(), ( s, e ) => e.CanExecute = myWebControl.CanGoBack() ) );
            CommandBindings.Add( new CommandBinding( NavigationCommands.BrowseForward, ( s, e ) => myWebControl.GoForward(), ( s, e ) => e.CanExecute = myWebControl.CanGoForward() ) );
            CommandBindings.Add( new CommandBinding( NavigationCommands.Refresh, ( s, e ) => myWebControl.Reload( false ), ( s, e ) => e.CanExecute = true ) );

            CommandBindings.Add( new CommandBinding( ApplicationCommands.Print,
                ( s, e ) => ApplicationCommands.Print.Execute( null, myWebControl ),
                ( s, e ) => e.CanExecute = true ) );

            CommandBindings.Add( new CommandBinding( WebControlCommands.CopyLinkAddress, ( s, e ) => myWebControl.CopyLinkAddress(), ( s, e ) => e.CanExecute = myWebControl.HasTargetURL ) );

            myWebControl.ShowCreatedWebView += OnShowCreatedWebView;
            myWebControl.InitializeView += myWebControl_InitializeView;
            myWebControl.ShowContextMenu += myWebControl_ShowContextMenu;

            var sourceDescriptor = DependencyPropertyDescriptor.FromProperty( WebControl.SourceProperty, typeof( WebControl ) );
            sourceDescriptor.AddValueChanged( myWebControl, OnSourceChanged );

            var titleDescriptor = DependencyPropertyDescriptor.FromProperty( WebControl.TitleProperty, typeof( WebControl ) );
            titleDescriptor.AddValueChanged( myWebControl, OnTitleChanged );

            var targetUrlDescriptor = DependencyPropertyDescriptor.FromProperty( WebControl.TargetURLProperty, typeof( WebControl ) );
            targetUrlDescriptor.AddValueChanged( myWebControl, OnTargetUrlChanged );

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public void Dispose()
        {
            if( myWebControl == null )
            {
                return;
            }

            CommandBindings.Clear();

            myWebControl.ShowCreatedWebView -= OnShowCreatedWebView;
            myWebControl.InitializeView -= myWebControl_InitializeView;
            myWebControl.ShowContextMenu -= myWebControl_ShowContextMenu;

            var sourceDescriptor = DependencyPropertyDescriptor.FromProperty( WebControl.SourceProperty, typeof( WebControl ) );
            sourceDescriptor.RemoveValueChanged( myWebControl, OnSourceChanged );

            var titleDescriptor = DependencyPropertyDescriptor.FromProperty( WebControl.TitleProperty, typeof( WebControl ) );
            titleDescriptor.RemoveValueChanged( myWebControl, OnTitleChanged );

            var targetUrlDescriptor = DependencyPropertyDescriptor.FromProperty( WebControl.TargetURLProperty, typeof( WebControl ) );
            targetUrlDescriptor.RemoveValueChanged( myWebControl, OnTargetUrlChanged );

            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;

            if( myTimer != null )
            {
                myTimer.Tick -= myTimer_Tick;
                myTimer = null;
            }

            myWebControl.Dispose();
            myWebControl = null;
        }

        // we need this magic because the WebControl is throwing an exception when this view gets unloaded and reloaded multiple times
        // while it is dragged in a tab of AvalonDock
        private void OnUnloaded( object sender, RoutedEventArgs e )
        {
            if( Mouse.LeftButton == MouseButtonState.Pressed )
            {
                var rect = new Rectangle();
                rect.Width = DesiredSize.Width;
                rect.Height = DesiredSize.Height;
                rect.Fill = new VisualBrush( myWebControl );

                myWebControlHost.Content = rect;

                myIsBeingDraging = true;
            }
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            InitializeContextMenus();

            if( !myIsBeingDraging )
            {
                return;
            }

            if( Mouse.LeftButton == MouseButtonState.Released )
            {
                myWebControlHost.Content = myWebControl;
                myIsBeingDraging = false;
                return;
            }

            if( myTimer == null )
            {
                myTimer = new DispatcherTimer();
                myTimer.Interval = TimeSpan.FromMilliseconds( 10 );
                myTimer.Tick += myTimer_Tick;
            }

            myTimer.Start();
        }

        private void InitializeContextMenus()
        {
            if( ContextMenu != null )
            {
                ContextMenu.PlacementTarget = this;
                ContextMenu.DataContext = DataContext;
            }

            if( LinkContextMenu != null )
            {
                LinkContextMenu.PlacementTarget = this;
                LinkContextMenu.DataContext = DataContext;
            }
        }

        private void myTimer_Tick( object sender, EventArgs e )
        {
            if( myIsBeingDraging && Mouse.LeftButton == MouseButtonState.Released )
            {
                myWebControlHost.Content = myWebControl;
                myIsBeingDraging = false;
                myTimer.Stop();
            }
        }

        private void myWebControl_InitializeView( object sender, WebViewEventArgs e )
        {
            var session = WebCore.Sessions.SingleOrDefault( s => s.DataPath.Equals( SessionFolder, StringComparison.OrdinalIgnoreCase ) );
            if( session == null )
            {
                session = WebCore.CreateWebSession( SessionFolder, Preferences );
            }
            myWebControl.WebSession = session;

            ( ( IViewConnector )DataContext ).View = this;
        }

        private void myWebControl_ShowContextMenu( object sender, Awesomium.Core.ContextMenuEventArgs e )
        {
            if( e.Info.HasLinkURL && LinkContextMenu != null )
            {
                LinkContextMenu.IsOpen = true;
            }
            else if( ContextMenu != null )
            {
                ContextMenu.IsOpen = true;
            }

            e.Handled = true;
        }

        private void OnTitleChanged( object sender, EventArgs e )
        {
            if( Title != myWebControl.Title )
            {
                Title = myWebControl.Title;
            }
        }

        private void OnSourceChanged( object sender, EventArgs e )
        {
            if( Source != myWebControl.Source )
            {
                Source = myWebControl.Source;
            }
        }

        private void OnTargetUrlChanged( object sender, EventArgs e )
        {
            if( TargetURL != myWebControl.TargetURL )
            {
                TargetURL = myWebControl.TargetURL;
            }
        }

        private void OnShowCreatedWebView( object sender, ShowCreatedWebViewEventArgs e )
        {
            if( OpenNewWindowRequestedCommand == null || !OpenNewWindowRequestedCommand.CanExecute( e.TargetURL ) )
            {
                return;
            }

            OpenNewWindowRequestedCommand.Execute( e.TargetURL );
        }

        public Uri Source
        {
            get { return ( Uri )GetValue( SourceProperty ); }
            set { SetValue( SourceProperty, value ); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register( "Source", typeof( Uri ),
            typeof( Browser ), new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );

        public Uri TargetURL
        {
            get { return ( Uri )GetValue( TargetURLProperty ); }
            set { SetValue( TargetURLProperty, value ); }
        }

        public static readonly DependencyProperty TargetURLProperty = DependencyProperty.Register( "TargetURL", typeof( Uri ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );

        public string Title
        {
            get { return ( string )GetValue( TitleProperty ); }
            set { SetValue( TitleProperty, value ); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register( "Title", typeof( string ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );

        public ICommand OpenNewWindowRequestedCommand
        {
            get { return ( ICommand )GetValue( OpenNewWindowRequestedCommandProperty ); }
            set { SetValue( OpenNewWindowRequestedCommandProperty, value ); }
        }

        public static readonly DependencyProperty OpenNewWindowRequestedCommandProperty = DependencyProperty.Register( "OpenNewWindowRequestedCommand", typeof( ICommand ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );

        public bool IsSourceView
        {
            get { return ( bool )GetValue( IsSourceViewProperty ); }
            internal set { SetValue( IsSourceViewProperty, value ); }
        }

        public static readonly DependencyProperty IsSourceViewProperty = DependencyProperty.Register( "IsSourceView", typeof( bool ),
            typeof( Browser ), new FrameworkPropertyMetadata( false ) );

        public WebPreferences Preferences
        {
            get { return ( WebPreferences )GetValue( PreferencesProperty ); }
            internal set { SetValue( PreferencesProperty, value ); }
        }

        public static readonly DependencyProperty PreferencesProperty = DependencyProperty.Register( "Preferences", typeof( WebPreferences ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );

        public string SessionFolder
        {
            get { return ( string )GetValue( SessionFolderProperty ); }
            internal set { SetValue( SessionFolderProperty, value ); }
        }

        public static readonly DependencyProperty SessionFolderProperty = DependencyProperty.Register( "SessionFolder", typeof( string ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );

        public ContextMenu LinkContextMenu
        {
            get { return ( ContextMenu )GetValue( LinkContextMenuProperty ); }
            set { SetValue( LinkContextMenuProperty, value ); }
        }

        public static readonly DependencyProperty LinkContextMenuProperty = DependencyProperty.Register( "LinkContextMenu", typeof( ContextMenu ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );

        public BrowserNavigationSettings NavigationSettings
        {
            get { return ( BrowserNavigationSettings )GetValue( NavigationSettingsProperty ); }
            set { SetValue( NavigationSettingsProperty, value ); }
        }

        public static readonly DependencyProperty NavigationSettingsProperty = DependencyProperty.Register( "NavigationSettings", typeof( BrowserNavigationSettings ),
            typeof( Browser ), new FrameworkPropertyMetadata( null ) );
    }
}
