using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Plainion.Notes.Services;
using Microsoft.Practices.Prism.Commands;
using Plainion.Windows;

namespace Plainion.Notes.ViewModels
{
    [Export]
    public class PageReadViewModel : PageViewModelBase
    {
        private FlowDocument myDocument;

        [ImportingConstructor]
        public PageReadViewModel( WikiService wikiService, PageNavigationService navigationService )
            : base( wikiService, navigationService )
        {
            EditCommand = new DelegateCommand( OnEdit );
        }

        public FlowDocument Document
        {
            get { return myDocument; }
            set
            {
                var oldDoc = myDocument;
                if( SetProperty( ref myDocument, value ) )
                {
                    if( oldDoc != null )
                    {
                        foreach( var link in oldDoc.GetVisuals().OfType<Hyperlink>() )
                        {
                            link.RequestNavigate -= OnHyperlinkRequestNavigate;
                        }
                    }

                    foreach( var link in myDocument.GetVisuals().OfType<Hyperlink>() )
                    {
                        link.RequestNavigate += OnHyperlinkRequestNavigate;
                    }
                }
            }
        }

        private void OnHyperlinkRequestNavigate( object sender, RequestNavigateEventArgs e )
        {
            if( e.Uri.IsAbsoluteUri )
            {
                Process.Start( new ProcessStartInfo( e.Uri.AbsoluteUri ) );
            }
            else
            {
                var url = new Uri( "page://local" + PageName.Namespace.AsPath + e.Uri.OriginalString );

                var decodedPath = HttpUtility.UrlDecode( url.AbsolutePath );
                var pageName = Wiki.AST.PageName.CreateFromPath( decodedPath );
                var args = HttpUtility.ParseQueryString( url.Query );
                var action = args.AllKeys.Contains( "action" ) ? args[ "action" ] : null;

                if( action == "new" )
                {
                    NavigationService.NavigateToCreate( pageName );
                }
                else
                {
                    NavigationService.NavigateToRead( pageName );
                }
            }


            e.Handled = true;
        }

        public ICommand EditCommand { get; private set; }

        private void OnEdit()
        {
            NavigationService.NavigateToEdit( PageName );
        }

        protected override void OnNavigatedToCompleted( PageNavigationParameters args )
        {
            Document = WikiService.Render( PageName );
        }
    }
}
