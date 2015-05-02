using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    [MetadataAttribute]
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class PageAttributeTransformerAttribute : ExportAttribute, IPageAttributeTransformerMetadata
    {
        /// <summary/>
        public PageAttributeTransformerAttribute( string qualifiedName )
            : base( typeof( IPageAttributeTransformer ) )
        {
            QualifiedAttributeName = qualifiedName;
        }

        /// <summary>
        /// Full qualified name of the page attribute.
        /// </summary>
        public string QualifiedAttributeName
        {
            get;
            private set;
        }
    }
}
