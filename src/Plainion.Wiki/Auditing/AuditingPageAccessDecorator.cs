using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki.Auditing
{
    public class AuditingPageAccessDecorator : PageAccessDecoratorBase
    {
        private IAuditingLog myLog;

        [ImportingConstructor]
        public AuditingPageAccessDecorator( IPageAccess pageAccess, IAuditingLog log )
            : base( pageAccess )
        {
            if ( log == null )
            {
                throw new ArgumentNullException( "log" );
            }

            myLog = log;
        }

        public override void Create( IPageDescriptor pageDescriptor )
        {
            myLog.Log( new CreateAction( pageDescriptor.Name ) );

            base.Create( pageDescriptor );
        }

        public override void Delete( IPageDescriptor pageDescriptor )
        {
            myLog.Log( new DeleteAction( pageDescriptor.Name ) );

            base.Delete( pageDescriptor );
        }

        public override void Update( IPageDescriptor pageDescriptor )
        {
            myLog.Log( new UpdateAction( pageDescriptor.Name ) );

            base.Update( pageDescriptor );
        }
    }
}
