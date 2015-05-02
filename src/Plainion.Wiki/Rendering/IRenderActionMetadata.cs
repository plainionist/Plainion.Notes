using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    public interface IRenderActionMetadata
    {
        /// <summary/>
        Type NodeType { get; }
    }
}
