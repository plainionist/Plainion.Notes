using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary/>
    public class AlwaysTrueFromClause : IFromClause
    {
        /// <summary/>
        public bool IsQueryFromPageAllowed( PageBody page )
        {
            return true;
        }
    }
}
