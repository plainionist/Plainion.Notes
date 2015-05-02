using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Linq.Dynamic;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Resolves value of identifiers.
    /// Supported identifier syntax: qualified names (contain a '.'), names 
    /// starting with '@'.
    /// </summary>
    [Serializable]
    public class QueryIdentifierResolver : IIdentifierResolver
    {
        /// <summary/>
        public bool HasValue( string identifier )
        {
            if ( string.IsNullOrEmpty( identifier ) )
            {
                return false;
            }

            // at compile time we dont know which identifiers
            // will really be available because the user can define those
            // in the pages all the time
            // -> accept all QualifiedNames
            // -> accept all attributes explicitly prefixed with "@"

            return identifier.Contains( '.' ) || identifier.StartsWith( "@" );
        }

        /// <summary/>
        public object GetValue( ParameterExpression iterator, string identifier )
        {
            var method = typeof( IQueryIterator ).GetMethod( "GetIdentifierValue" );
            return Expression.Call( iterator, method, Expression.Constant( identifier ) );
        }
    }
}
