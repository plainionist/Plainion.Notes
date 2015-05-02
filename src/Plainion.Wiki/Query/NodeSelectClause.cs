using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Selects directly the nodes passed in from the query.
    /// Creates <see cref="QueryMatch"/>es which directly reference the nodes
    /// passed in "DisplayText".
    /// </summary>
    public class NodeSelectClause : ISelectClause
    {
        /// <summary/>
        public IEnumerable<QueryMatch> Select( IEnumerable<PageLeaf> nodes )
        {
            return nodes.Select( node => new QueryMatch( node ) ).ToList();
        }
    }
}
