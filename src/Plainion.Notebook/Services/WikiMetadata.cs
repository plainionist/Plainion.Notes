using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Http;
using Plainion.Wiki.Rendering;

namespace Plainion.Notebook.Services
{
    [Export]
    class WikiMetadata
    {
        private PageLayoutDescriptor myPageLayoutDescriptor;

        [ImportingConstructor]
        public WikiMetadata(PageLayoutDescriptor pageLayoutDescriptor)
        {
            myPageLayoutDescriptor = pageLayoutDescriptor;

            PrintPreviewPageName = PageName.Create("Print preview");
        }

        public bool IsContent(PageName page)
        {
            return myPageLayoutDescriptor.Header != page
                && myPageLayoutDescriptor.Footer != page
                && myPageLayoutDescriptor.SideBar != page
                && !IsTool(page);
        }

        public bool IsTool(PageName page)
        {
            return page.Name == "Page.Navigation"
                || page.Name == PageNames.SiteSearchResults;
        }

        public PageName PrintPreviewPageName { get; private set; }
    }
}
