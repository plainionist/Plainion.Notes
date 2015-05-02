using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Plainion.Wiki.Auditing
{
    [Export( typeof( IAuditingLog ) )]
    public class NullAuditingLog : IAuditingLog
    {
        public void Log( IAuditingAction action )
        {
        }

        public IEnumerable<IAuditingAction> Actions
        {
            get { return Enumerable.Empty<IAuditingAction>(); }
        }
    }
}
