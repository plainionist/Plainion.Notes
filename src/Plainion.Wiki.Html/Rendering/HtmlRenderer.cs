using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Plainion.Wiki.Rendering;
using Plainion;

namespace Plainion.Wiki.Html.Rendering
{
    public class HtmlRenderer : AbstractRenderer, IHtmlRenderActionContext
    {
        private HtmlStylesheet myStylesheet;
        private HtmlRenderActionCatalog myCatalog;

        [ImportingConstructor]
        public HtmlRenderer( HtmlRenderActionCatalog catalog )
        {
            Contract.RequiresNotNull( catalog, "catalog" );

            myCatalog = catalog;
            myStylesheet = new HtmlStylesheet();
        }

        protected override IDictionary<Type, IRenderAction> RenderActions
        {
            get { return myCatalog.Plugins; }
        }

        [Import( AllowDefault = true )]
        public HtmlStylesheet Stylesheet
        {
            get { return myStylesheet; }
            set
            {
                myStylesheet = value ?? new HtmlStylesheet();
            }
        }
    }
}
