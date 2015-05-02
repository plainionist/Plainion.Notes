using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    public interface IRenderingStep
    {
        /// <summary/>
        PageLeaf Transform( PageLeaf node, EngineContext context );
    }
}
