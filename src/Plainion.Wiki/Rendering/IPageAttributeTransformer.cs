using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    public interface IPageAttributeTransformer
    {
        /// <summary/>
        void Transform( PageAttribute pageAttribute, EngineContext context );
    }
}
