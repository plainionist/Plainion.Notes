using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    public interface IRenderAction
    {
        /// <summary/>
        void Render( PageLeaf node, IRenderActionContext context );
    }
}
