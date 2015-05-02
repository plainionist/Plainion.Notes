using System;
using System.ComponentModel.Composition;
using Plainion.Notebook.Model;
using Plainion.AppFw.Wpf.Services;

namespace Plainion.Notebook.ViewModels
{
    class SearchResultsViewModel : BrowserToolsViewModel
    {
        public const string ToolContentId = "SearchResultsTool";

        [ImportingConstructor]
        public SearchResultsViewModel( IProjectService<Project> projectService, IPageNavigation navigation )
            : base( projectService, navigation, "Search Results", ToolContentId )
        {
            Uri = new Uri( "about:blank" );
        }
    }
}
