using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Defines a generic matcher for the <see cref="QueryEngine"/>
    /// </summary>
    public interface IQueryMatcher
    {
        /// <summary/>
        IEnumerable<QueryMatch> Match( PageHandle page );
    }
}
