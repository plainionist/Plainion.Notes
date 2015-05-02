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
    [PageAttributeTransformer( "page.reverselinks" )]
    public class ReverseLinksTransformer : IPageAttributeTransformer
    {
        /// <summary/>
        public void Transform( PageAttribute pageAttribute, EngineContext context )
        {
            var pageName = pageAttribute.GetNameOfPage();
            if ( pageName == null )
            {
                throw new InvalidOperationException( "Could not get name of the page" );
            }

            var hits = context.Query.ReferencingPages( pageName );

            var content = ContentBuilder.BuildQueryResultNoBullets( hits, "not referenced" );

            pageAttribute.Parent.ReplaceChild( pageAttribute, content );
        }
    }
}
