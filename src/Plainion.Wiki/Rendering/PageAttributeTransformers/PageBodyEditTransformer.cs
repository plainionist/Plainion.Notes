using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Rendering.PageAttributeTransformers
{
    /// <summary/>
    [PageAttributeTransformer( "pagebody.edit" )]
    public class PageBodyEditTransformer : IPageAttributeTransformer
    {
        /// <summary/>
        public void Transform( PageAttribute pageAttribute, EngineContext context )
        {
            var pageName = pageAttribute.GetParentOfType<PageBody>().Name;

            var url = pageName != null ? pageName.FullName : string.Empty;
            url += "?action=edit";

            var link = new Link( url, "Edit" );
            link.IsStatic = true;

            pageAttribute.Parent.ReplaceChild( pageAttribute, link );
        }
    }
}
