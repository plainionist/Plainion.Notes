using System;
using System.Linq.Expressions;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary/>
    public class DynamicLinqWhereClause : IWhereClause
    {
        private Func<QueryIterator, bool> myPredicate;

        /// <summary/>
        public DynamicLinqWhereClause( string expression )
        {
            var resolver = new QueryIdentifierResolver();

            var expr = (Expression<Func<QueryIterator, bool>>)Microsoft.Linq.Dynamic.DynamicExpression
                .ParseLambda( typeof( QueryIterator ), typeof( bool ), expression, resolver );

            myPredicate = expr.Compile();
        }

        /// <summary/>
        public DynamicLinqWhereClause( Func<PageLeaf, bool> predicate )
        {
            myPredicate = iterator => predicate( iterator.CurrentNode );
        }

        /// <summary/>
        public bool Where( IQueryIterator iterator )
        {
            if ( !( iterator is QueryIterator ) )
            {
                throw new ArgumentException( "Wrong iterator type. Use CreateIterator() to create the iterator instance" );
            }

            return myPredicate( (QueryIterator)iterator );
        }

        /// <summary/>
        public IQueryIterator CreateIterator( CompiledQuery query )
        {
            return new QueryIterator( query );
        }
    }
}
