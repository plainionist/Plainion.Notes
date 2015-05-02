using System.ComponentModel.Composition;
using Plainion.Notebook.Model;
using Plainion.Notebook.Services;
using Plainion.AppFw.Wpf.Services;

namespace Plainion.Notebook.ViewModels
{
    class NavigationViewModel : BrowserToolsViewModel
    {
        public const string ToolContentId = "NavigationTool";

        [ImportingConstructor]
        public NavigationViewModel( WikiService wikiService, IProjectService<Project> projectService, IPageNavigation navigation )
            : base( projectService, navigation, "Navigation", ToolContentId )
        {
            Uri = wikiService.GetUriFromPath( "Page.Navigation" );
        }
    }
}
