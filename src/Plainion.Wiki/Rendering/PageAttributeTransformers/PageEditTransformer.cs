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
    [PageAttributeTransformer( "page.edit" )]
    public class PageEditTransformer : IPageAttributeTransformer
    {
        /// <summary/>
        public void Transform( PageAttribute pageAttribute, EngineContext context )
        {
            var pageName = pageAttribute.GetNameOfPage();

            var url = pageName != null ? pageName.FullName : string.Empty;
            url += "?action=edit";

            var link = new Link( url, "Edit" );
            link.IsStatic = true;

            pageAttribute.Parent.ReplaceChild( pageAttribute, link );
        }
    }
}
