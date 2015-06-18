using System.IO;
using System.Linq;
using Plainion.Wiki;
using Plainion.Wiki.AST;

namespace Plainion.Notebook.Services
{
    static class EngineExtensions
    {
        // TODO: no "edit" in this page ... but currently only possible by skipping the entire header
        public static void RenderAllContentIntoOnePage(this IEngine self, WikiMetadata wikiMetadata, PageName targetPageName, Stream output)
        {
            var root = new PageBody(targetPageName);

            var contentPages = self.Query.All()
                .Where(n => wikiMetadata.IsContent(n));

            foreach (var pageName in contentPages)
            {
                var page = self.Get(pageName);
                root.Consume(new Headline(pageName.Name, 1));
                root.Consume(page);
            }

            var renderPageNameAsHeadlineOld = self.Config.RenderPageNameAsHeadline;
            try
            {
                self.Render(root, output);
            }
            finally
            {
                self.Config.RenderPageNameAsHeadline = renderPageNameAsHeadlineOld;
            }
        }
    }
}
