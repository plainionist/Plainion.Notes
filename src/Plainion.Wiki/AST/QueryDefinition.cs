using Plainion.Wiki.Query;
using System;
using System.Runtime.Serialization;
using System.Text;
using Plainion;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents a query formulated inside a page.
    /// </summary>
    [Serializable]
    public class QueryDefinition : PageAttribute
    {
        /// <summary/>
        public static readonly string AttributeType = "query";

        /// <summary/>
        public QueryDefinition( string whereExpr )
            : this( whereExpr, null )
        {
        }

        /// <summary/>
        public QueryDefinition( string whereExpr, string selectExpr )
            : this( whereExpr, selectExpr, null )
        {
        }

        /// <summary/>
        public QueryDefinition( string whereExpr, string selectExpr, string fromExpr )
            : base( AttributeType, null, CreateValue( whereExpr, selectExpr, fromExpr ) )
        {
            WhereExpression = whereExpr != null ? whereExpr.Trim() : null;
            if ( string.IsNullOrEmpty( WhereExpression ) )
            {
                throw new ArgumentNullException( "whereExpr" );
            }

            SelectExpression = selectExpr != null ? selectExpr.Trim() : null;
            FromExpression = fromExpr != null ? fromExpr.Trim() : null;
        }

        /// <summary/>
        public string WhereExpression
        {
            get;
            private set;
        }

        /// <summary/>
        public string SelectExpression
        {
            get;
            private set;
        }

        /// <summary/>
        public string FromExpression
        {
            get;
            private set;
        }

        private static string CreateValue( string whereExpr, string selectExpr, string fromExpr )
        {
            var sb = new StringBuilder();

            sb.Append( whereExpr );

            if ( selectExpr != null )
            {
                sb.Append( ";" );
                sb.Append( selectExpr );
            }

            if ( fromExpr != null )
            {
                sb.Append( ";" );
                sb.Append( fromExpr );
            }

            return sb.ToString();
        }
    }
}
