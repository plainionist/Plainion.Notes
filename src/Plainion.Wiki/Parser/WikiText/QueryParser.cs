using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Collections;

namespace Plainion.Wiki.Parser
{
    /// <summary/>
    public class QueryParser
    {
        private StringBuilder myWhereExpr;
        private StringBuilder mySelectExpr;
        private StringBuilder myFromExpr;

        private Queue<StringBuilder> myExprQueue;

        /// <summary/>
        public QueryParser()
        {
            myWhereExpr = new StringBuilder();
            mySelectExpr = new StringBuilder();
            myFromExpr = new StringBuilder();

            myExprQueue = new Queue<StringBuilder>();
        }

        /// <summary/>
        public QueryDefinition Parse( string queryDefinition )
        {
            Initialize();

            var currentClause = myExprQueue.Dequeue();

            var previousCh = '\0';
            foreach ( var ch in queryDefinition.ToCharArray() )
            {
                if ( ch == ';' )
                {
                    if ( previousCh == '\\' )
                    {
                        currentClause.Remove( currentClause.Length - 1, 1 );
                        currentClause.Append( ch );
                    }
                    else
                    {
                        if ( !myExprQueue.Any() )
                        {
                            throw new Exception( "Unexpected count of query expression separator char ';'. Escape the separator char inside the query" );
                        }
                        currentClause = myExprQueue.Dequeue();
                    }
                }
                else
                {
                    currentClause.Append( ch );
                }

                previousCh = ch;
            }


            return new QueryDefinition( myWhereExpr.ToString(), mySelectExpr.ToString(), myFromExpr.ToString() );
        }

        private void Initialize()
        {
            myWhereExpr.Remove( 0, myWhereExpr.Length );
            mySelectExpr.Remove( 0, mySelectExpr.Length );
            myFromExpr.Remove( 0, myFromExpr.Length );

            myExprQueue.Clear();
            myExprQueue.Enqueue( myWhereExpr );
            myExprQueue.Enqueue( mySelectExpr );
            myExprQueue.Enqueue( myFromExpr );
        }
    }
}
