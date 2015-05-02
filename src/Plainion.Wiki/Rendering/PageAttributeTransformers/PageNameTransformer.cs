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
    [PageAttributeTransformer( "page.name" )]
    public class PageNameTransformer : IPageAttributeTransformer
    {
        /// <summary/>
        public void Transform( PageAttribute pageAttribute, EngineContext context )
        {
            var pageName = pageAttribute.GetNameOfPage();
            if ( pageName == null )
            {
                return;
            }

            pageAttribute.Parent.ReplaceChild( pageAttribute, new PlainText( pageName.Name ) );
        }
    }
}
