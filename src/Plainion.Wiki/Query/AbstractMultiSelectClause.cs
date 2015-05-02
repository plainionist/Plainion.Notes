using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Ensures that no duplicates will be returned.
    /// </summary>
    public abstract class AbstractMultiSelectClause : ISelectClause
    {
        private class SelectedNodeHandleComparer : EqualityComparer<SelectedNodeHandle>
        {
            public override bool Equals( SelectedNodeHandle x, SelectedNodeHandle y )
            {
                return object.ReferenceEquals( x.SelectedNode, y.SelectedNode );
            }

            public override int GetHashCode( SelectedNodeHandle obj )
            {
                return obj.SelectedNode.GetHashCode();
            }
        }

        /// <summary>
        /// This class is used by derived "ISelectClause" classes to tell 
        /// which node they would select and how a QueryMatch would be created 
        /// based on it.
        /// Then the AbstractMultiSelectClause removes duplicates from
        /// the selected nodes and calls the "QueryMatchCreator" to create
        /// the final QueryMatches.
        /// </summary>
        protected class SelectedNodeHandle
        {
            /// <summary/>
            public PageLeaf SelectedNode { get; set; }

            /// <summary/>
            public Func<QueryMatch> QueryMatchCreator { get; set; }

            /// <summary/>
            public static SelectedNodeHandle CreatePageMatch( PageBody pageBody )
            {
                if ( pageBody == null )
                {
                    throw new InvalidOperationException( "Cannot create page match from a node without PageBody parent" );
                }

                return new SelectedNodeHandle()
                    {
                        SelectedNode = pageBody,
                        QueryMatchCreator = () => QueryMatch.CreatePageMatch( pageBody.Name )
                    };
            }
        }

        /// <summary/>
        public IEnumerable<QueryMatch> Select( IEnumerable<PageLeaf> nodes )
        {
            var selectedNodes = nodes.Select( node => Select( node ) );

            // a selector might return "null" to indicate that it cannot 
            // really select what it should select (e.g. AttributeValueSelectClause
            // cannot perform a selection on a non PageAttribute node)
            var validSelectedNodes = selectedNodes.Where( node => node != null );

            return validSelectedNodes
                .Distinct( new SelectedNodeHandleComparer() )
                .Select( lazyMatch => lazyMatch.QueryMatchCreator() )
                .ToList();
        }

        /// <summary/>
        protected abstract SelectedNodeHandle Select( PageLeaf node );
    }
}
