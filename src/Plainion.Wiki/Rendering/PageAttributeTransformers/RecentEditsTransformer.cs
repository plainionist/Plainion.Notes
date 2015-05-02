using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.Query;

namespace Plainion.Wiki.Rendering.PageAttributeTransformers
{
    /// <summary/>
    [PageAttributeTransformer( "site.recentedits" )]
    public class RecentEditsTransformer : IPageAttributeTransformer
    {
        private const int MaxNumberOfPages = 5;

        /// <summary/>
        public void Transform( PageAttribute pageAttribute, EngineContext context )
        {
            var hits = QueryAuditingLog( context.AuditingLog );

            var content = ContentBuilder.BuildQueryResultNoBullets( hits, "n.a." );

            pageAttribute.Parent.ReplaceChild( pageAttribute, content );
        }

        private IEnumerable<QueryMatch> QueryAuditingLog( IAuditingLog auditingLog )
        {
            if ( auditingLog == null )
            {
                return QueryMatch.Bundle();
            }

            return GetRecentlyUpdatedPages( auditingLog )
                .Select( pageName => QueryMatch.CreatePageMatch( pageName ) )
                .ToList();
        }

        private IEnumerable<PageName> GetRecentlyUpdatedPages( IAuditingLog auditingLog )
        {
            var recentlyUpdatedPages = GetRecentUpdateActions( auditingLog )
                .Select( action => action.RelatedPage );

            return recentlyUpdatedPages
                .Distinct()
                .Take( MaxNumberOfPages );
        }

        private IEnumerable<IAuditingAction> GetRecentUpdateActions( IAuditingLog auditingLog )
        {
            return auditingLog.Actions
                .Where( action => action is CreateAction || action is UpdateAction )
                .Reverse();
        }
    }
}
