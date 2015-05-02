using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki.Xaml.Rendering.RenderActions
{
    [XamlRenderAction( typeof( LineBreak ) )]
    public class LineBreakRenderAction : GenericRenderAction<LineBreak>
    {
        protected override void Render( LineBreak lineBreak )
        {
            WriteLine( "<LineBreak/>" );
        }
    }
}
