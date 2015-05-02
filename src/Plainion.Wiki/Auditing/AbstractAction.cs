using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Auditing
{
    /// <summary/>
    public class AbstractAction : IAuditingAction
    {
        /// <summary/>
        protected AbstractAction( PageName pageName )
        {
            if ( pageName == null )
            {
                throw new ArgumentNullException( "pageName" );
            }

            RelatedPage = pageName;
        }

        /// <summary/>
        public PageName RelatedPage
        {
            get;
            private set;
        }
    }
}
