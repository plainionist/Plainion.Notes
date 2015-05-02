using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Auditing
{
    /// <summary/>
    public class UpdateAction : AbstractAction
    {
        /// <summary/>
        public UpdateAction( PageName pageName )
            : base( pageName )
        {
        }
    }
}
