using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering
{
    [MetadataAttribute]
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class XamlRenderActionAttribute : RenderActionAttribute
    {
        public XamlRenderActionAttribute( Type nodeType )
            : base( typeof( XamlRenderActionAttribute ), nodeType )
        {
        }
    }
}
