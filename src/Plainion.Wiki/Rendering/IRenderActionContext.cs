using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    public interface IRenderActionContext
    {
        /// <summary/>
        RenderingContext RenderingContext
        {
            get;
        }

        /// <summary>
        /// Root of the AST to be rendered.
        /// Usually its the <see cref="Page"/> but could also be a <see cref="PageBody"/>
        /// or any other AST node.
        /// </summary>
        PageLeaf Root
        {
            get;
        }

        /// <summary>
        /// Triggers another render action in the current rendering context.
        /// </summary>
        void Render( PageLeaf node );
    }
}
