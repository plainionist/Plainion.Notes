using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.Auditing
{
    public interface IAuditingLog
    {
        IEnumerable<IAuditingAction> Actions { get; }
        
        void Log( IAuditingAction action );
    }
}
