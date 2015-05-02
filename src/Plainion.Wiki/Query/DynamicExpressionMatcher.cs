using System;
using System.Collections.Generic;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary/>
    public class DynamicExpressionMatcher : IQueryMatcher
    {
        private DynamicQueryExecutor myExecutor;

        /// <summary/>
        public DynamicExpressionMatcher( CompiledQuery query )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( "query" );
            }

            myExecutor = new DynamicQueryExecutor( query );
        }

        /// <summary/>
        public IEnumerable<QueryMatch> Match( PageHandle page )
        {
            return myExecutor.Execute( page.Body );
        }
    }
}
