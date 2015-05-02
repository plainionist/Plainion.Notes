using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.Query;

namespace Plainion.Wiki.Http
{
    /// <summary/>
    public interface IViewContext
    {
        /// <summary/>
        IEngine Engine { get; }
    }
}
