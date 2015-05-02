﻿using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( TextBlock ) )]
    public class TextBlockRenderAction : GenericRenderAction<TextBlock>
    {
        /// <summary/>
        protected override void Render( TextBlock textBlock )
        {
            Render( textBlock.Children );
        }
    }
}
