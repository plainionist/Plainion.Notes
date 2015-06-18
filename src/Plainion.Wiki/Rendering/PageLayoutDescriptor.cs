using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Resources;

namespace Plainion.Wiki.Rendering
{
    [Export]
    public class PageLayoutDescriptor
    {
        public PageLayoutDescriptor()
        {
            Header = PageName.Create(ResourceNames.PageHeader);
            Footer = PageName.Create(ResourceNames.PageFooter);
            SideBar = PageName.Create(ResourceNames.PageSideBar);
        }

        public PageName Header { get; set; }

        public PageName Footer { get; set; }

        public PageName SideBar { get; set; }
    }
}
