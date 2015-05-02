using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;

namespace Plainion.Wiki.Utils
{
    /// <summary>
    /// Builds AST sub graphs from the given input.
    /// </summary>
    public class ContentBuilder
    {
        /// <summary/>
        // TODO: maybe we should work with settings here?
        public static Content BuildQueryResultNoBullets( IEnumerable<QueryMatch> hits, string noResultsText )
        {
            var content = new Content();
            if ( hits.Any() )
            {
                foreach ( var hit in hits )
                {
                    content.Consume( hit.DisplayText );
                    content.Consume( new LineBreak() );
                }
            }
            else
            {
                content.Consume( new PlainText( noResultsText ) );
            }

            return content;
        }

        /// <summary/>
        public static Content BuildQueryResult( IEnumerable<QueryMatch> hits, string noResultsText )
        {
            PageLeaf queryResult = ContentBuilder.BuildQueryResult( hits );
            if ( queryResult == null )
            {
                queryResult = new PlainText( noResultsText );
            }

            return new Content( queryResult );
        }

        /// <summary/>
        public static BulletList BuildQueryResult( IEnumerable<QueryMatch> hits )
        {
            if ( !hits.Any() )
            {
                return null;
            }

            var list = new BulletList();

            foreach ( var hit in hits )
            {
                list.Consume( new ListItem( new TextBlock( hit.DisplayText ) ) );
            }

            return list;
        }
    }
}
