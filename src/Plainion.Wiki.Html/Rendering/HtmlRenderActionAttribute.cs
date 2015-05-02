using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering
{
    /// <summary/>
    [MetadataAttribute]
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class HtmlRenderActionAttribute : RenderActionAttribute
    {
        /// <summary/>
        public HtmlRenderActionAttribute( Type nodeType )
            : base( typeof( HtmlRenderActionAttribute ), nodeType )
        {
        }
    }
}
