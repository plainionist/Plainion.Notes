using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Auditing
{
    /// <summary/>
    public class DeleteAction : AbstractAction
    {
        /// <summary/>
        public DeleteAction( PageName pageName )
            : base( pageName )
        {
        }
    }
}
