using Plainion.Wiki.AST;
using Microsoft.Practices.Prism.Regions;

namespace Plainion.Notes.Services
{
    public class PageNavigationParameters
    {
        public PageNavigationParameters()
            : this( new NavigationParameters() )
        {
        }

        public PageNavigationParameters( NavigationParameters parameters )
        {
            Parameters = parameters;
        }

        public NavigationParameters Parameters
        {
            get;
            private set;
        }

        public PageName PageName
        {
            get { return ( PageName )Parameters[ "PageName" ]; }
            set { Parameters.Add( "PageName", value ); }
        }

        public bool CreateNew
        {
            get { return ( bool )Parameters[ "CreateNew" ]; }
            set { Parameters.Add( "CreateNew", value ); }
        }
    }
}
