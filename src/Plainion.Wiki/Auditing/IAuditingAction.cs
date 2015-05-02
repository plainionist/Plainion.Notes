using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Auditing
{
    /// <summary/>
    public interface IAuditingAction
    {
        /// <summary/>
        PageName RelatedPage { get; }
    }
}
