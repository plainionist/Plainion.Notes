using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Xaml.Rendering
{
    /// <summary>
    /// Resolves and contains all configured RenderActions for the XamlRenderer.
    /// </summary>
    [Export( typeof( XamlRenderActionCatalog ) )]
    public class XamlRenderActionCatalog : AbstractPluginCatalog<Type, IRenderAction, IRenderActionMetadata>
    {
#pragma warning disable 649
        [ImportMany( typeof( XamlRenderActionAttribute ) )]
        private IEnumerable<Lazy<IRenderAction, IRenderActionMetadata>> myImports;
#pragma warning restore 649

        protected override IEnumerable<Lazy<IRenderAction, IRenderActionMetadata>> Imports
        {
            get { return myImports; }
        }

        protected override Type GetKey( IRenderActionMetadata metadata )
        {
            return metadata.NodeType;
        }
    }
}
