using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Auditing
{
    /// <summary/>
    public class CreateAction : AbstractAction
    {
        /// <summary/>
        public CreateAction( PageName pageName )
            : base( pageName )
        {
        }
    }
}
