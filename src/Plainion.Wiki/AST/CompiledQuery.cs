using Plainion.Wiki.Query;
using System;
using System.Runtime.Serialization;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents a query formulated inside a page.
    /// The query is parsed directly after creation and stored in compiled form.
    /// </summary>
    [Serializable]
    public class CompiledQuery : PageLeaf
    {
        /// <summary/>
        public CompiledQuery( QueryDefinition query,
            IWhereClause whereClause, ISelectClause selectClause, IFromClause fromClause )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( "query" );
            }
            if ( whereClause == null )
            {
                throw new ArgumentNullException( "whereClause" );
            }
            if ( selectClause == null )
            {
                throw new ArgumentNullException( "selectClause" );
            }
            if ( fromClause == null )
            {
                throw new ArgumentNullException( "fromClause" );
            }

            Definition = query;
            WhereClause = whereClause;
            SelectClause = selectClause;
            FromClause = fromClause;
        }

        /// <summary/>
        public QueryDefinition Definition
        {
            get;
            private set;
        }

        /// <summary/>
        public IWhereClause WhereClause
        {
            get;
            private set;
        }

        /// <summary/>
        public ISelectClause SelectClause
        {
            get;
            private set;
        }

        /// <summary/>
        public IFromClause FromClause
        {
            get;
            private set;
        }
    }
}
