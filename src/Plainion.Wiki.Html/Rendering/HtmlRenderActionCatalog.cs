using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Html.Rendering
{
    /// <summary>
    /// Resolves and contains all configured RenderActions for the HtmlRenderer.
    /// </summary>
    [Export( typeof( HtmlRenderActionCatalog ) )]
    public class HtmlRenderActionCatalog : AbstractPluginCatalog<Type, IRenderAction, IRenderActionMetadata>
    {
#pragma warning disable 649
        [ImportMany( typeof( HtmlRenderActionAttribute ) )]
        private IEnumerable<Lazy<IRenderAction, IRenderActionMetadata>> myImports;
#pragma warning restore 649

        /// <summary/>
        protected override IEnumerable<Lazy<IRenderAction, IRenderActionMetadata>> Imports
        {
            get { return myImports; }
        }

        /// <summary/>
        protected override Type GetKey( IRenderActionMetadata metadata )
        {
            return metadata.NodeType;
        }
    }
}
