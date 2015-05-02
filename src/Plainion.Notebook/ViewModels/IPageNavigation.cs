using System;

namespace Plainion.Notebook.ViewModels
{
    interface IPageNavigation
    {
        void OpenPageSourceView( Uri url );

        void OpenInNewTab( Uri url );

        void OpenInExternalBrowser( Uri url );

        void OpenInActiveTab( Uri url );
    }
}
