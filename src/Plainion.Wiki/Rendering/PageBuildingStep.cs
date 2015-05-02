using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Resources;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    [RenderingStep( RenderingStage.BuildUpPage )]
    public class PageBuildingStep : IRenderingStep
    {
        /// <summary/>
        public PageBuildingStep()
        {
            HeaderPageName = ResourceNames.PageHeader;
            FooterPageName = ResourceNames.PageFooter;
            SideBarPageName = ResourceNames.PageSideBar;
        }

        /// <summary/>
        public string HeaderPageName
        {
            get;
            set;
        }

        /// <summary/>
        public string FooterPageName
        {
            get;
            set;
        }

        /// <summary/>
        public string SideBarPageName
        {
            get;
            set;
        }

        /// <summary/>
        public PageLeaf Transform( PageLeaf node, EngineContext context )
        {
            var body = node as PageBody;
            if ( body == null )
            {
                throw new ArgumentException( "only PageBody as input supported" );
            }

            var page = new Page( body.Name );
            page.Content = body;

            page.Header = context.GetPage( PageName.Create( HeaderPageName ) );
            page.Footer = context.GetPage( PageName.Create( FooterPageName ) );
            page.SideBar = context.GetPage( PageName.Create( SideBarPageName ) );

            return page;
        }
    }
}
