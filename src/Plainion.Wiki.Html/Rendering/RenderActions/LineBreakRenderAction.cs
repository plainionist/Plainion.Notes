using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Html.Rendering.RenderActions
{
    /// <summary/>
    [HtmlRenderAction( typeof( LineBreak ) )]
    public class LineBreakRenderAction : GenericRenderAction<LineBreak>
    {
        /// <summary/>
        protected override void Render( LineBreak lineBreak )
        {
            WriteLine( "<br/>" );
        }
    }
}
