using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Transforms matched nodes of a page into QueryMatches according to
    /// select clause.
    /// </summary>
    public interface ISelectClause
    {
        /// <summary/>
        IEnumerable<QueryMatch> Select( IEnumerable<PageLeaf> nodes );
    }
}
