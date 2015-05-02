using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Auditing
{
    [Export( typeof( IAuditingLog ) )]
    public class DefaultAuditingLog : IAuditingLog
    {
        private Queue<IAuditingAction> myActions;

        public DefaultAuditingLog()
        {
            myActions = new Queue<IAuditingAction>();
        }

        public void Log( IAuditingAction action )
        {
            if ( action == null )
            {
                throw new ArgumentNullException( "action" );
            }

            myActions.Enqueue( action );
        }

        public IEnumerable<IAuditingAction> Actions
        {
            get { return myActions; }
        }
    }
}
