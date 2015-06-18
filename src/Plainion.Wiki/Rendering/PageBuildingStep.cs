using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Resources;

namespace Plainion.Wiki.Rendering
{
    [RenderingStep(RenderingStage.BuildUpPage)]
    public class PageBuildingStep : IRenderingStep
    {
        private PageLayoutDescriptor myPageLayoutDescriptor;

        [ImportingConstructor]
        public PageBuildingStep(PageLayoutDescriptor pageLayoutDescriptor)
        {
            myPageLayoutDescriptor = pageLayoutDescriptor;
        }

        public PageLeaf Transform(PageLeaf node, EngineContext context)
        {
            var body = node as PageBody;
            if (body == null)
            {
                throw new ArgumentException("only PageBody as input supported");
            }

            var page = new Page(body.Name);
            page.Content = body;

            page.Header = context.GetPage(myPageLayoutDescriptor.Header);
            page.Footer = context.GetPage(myPageLayoutDescriptor.Footer);
            page.SideBar = context.GetPage(myPageLayoutDescriptor.SideBar);

            return page;
        }
    }
}
