using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Defines to which pages the query shall be applied to.
    /// </summary>
    public interface IFromClause
    {
        /// <summary/>
        bool IsQueryFromPageAllowed( PageBody page );
    }
}
