using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Executes a given <see cref="CompiledQuery"/> on a page by creating
    /// <see cref="QueryMatch"/>es for each match.
    /// </summary>
    public class DynamicQueryExecutor
    {
        /// <summary/>
        public DynamicQueryExecutor( CompiledQuery query )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( "query" );
            }

            Query = query;
        }

        /// <summary/>
        public CompiledQuery Query
        {
            get;
            private set;
        }

        /// <summary/>
        public IEnumerable<QueryMatch> Execute( PageBody page )
        {
            if ( !Query.FromClause.IsQueryFromPageAllowed( page ) )
            {
                return QueryMatch.Bundle();
            }

            var allNodes = page.GetFlattenedTree();
            var filteredNodes = ApplyWhereClause( allNodes );
            var selectedMatches = Query.SelectClause.Select( filteredNodes ).ToList();

            return selectedMatches;
        }

        private IEnumerable<PageLeaf> ApplyWhereClause( IEnumerable<PageLeaf> nodes )
        {
            var iterator = Query.WhereClause.CreateIterator( Query );
            foreach ( var node in nodes )
            {
                iterator.CurrentNode = node;

                if ( Query.WhereClause.Where( iterator ) )
                {
                    yield return node;
                }
            }
        }
    }
}
