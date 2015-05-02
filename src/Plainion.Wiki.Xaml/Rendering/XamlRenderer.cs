using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Plainion.Wiki.Rendering;
using Plainion;

namespace Plainion.Wiki.Xaml.Rendering
{
    public class XamlRenderer : AbstractRenderer, IRenderActionContext
    {
        private XamlRenderActionCatalog myCatalog;

        [ImportingConstructor]
        public XamlRenderer( XamlRenderActionCatalog catalog )
        {
            Contract.RequiresNotNull( catalog, "catalog" );

            myCatalog = catalog;
        }

        protected override IDictionary<Type, IRenderAction> RenderActions
        {
            get { return myCatalog.Plugins; }
        }
    }
}
