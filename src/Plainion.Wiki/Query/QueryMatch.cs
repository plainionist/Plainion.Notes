using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using Plainion.Collections;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Defines a match (positive result) of a query.
    /// <seealso cref="QueryEngine"/>
    /// </summary>
    public class QueryMatch
    {
        /// <summary/>
        public static readonly IEnumerable<QueryMatch> None = new List<QueryMatch>();

            /// <summary/>
        public QueryMatch( PageLeaf displayText )
        {
            if ( displayText == null )
            {
                throw new ArgumentNullException( "displayText" );
            }

            DisplayText = displayText;
        }

        /// <summary/>
        public PageLeaf DisplayText
        {
            get;
            private set;
        }

        /// <summary/>
        public static QueryMatch CreatePageMatch( PageName pageName )
        {
            return new QueryMatch( new Link( pageName.FullName, pageName.FullName.Substring( 1 ) ) );
        }

        /// <summary/>
        public static IEnumerable<QueryMatch> Bundle( params QueryMatch[] matches )
        {
            return matches.Where( m => m != null ).ToArray();
        }
    }
}
