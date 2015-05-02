using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Selects the value of the matched attributed.
    /// Returns null if the given node is no <see cref="PageAttribute"/>.
    /// </summary>
    public class AttributeValueSelectClause : AbstractMultiSelectClause
    {
        /// <summary/>
        protected override SelectedNodeHandle Select( PageLeaf node )
        {
            var attribute = node as PageAttribute;
            if ( attribute == null )
            {
                // node is not a PageAttribute
                // -> return null to signal that this is not valid
                return null;
            }

            var pageBody = node.GetParentOfType<PageBody>();
            if ( pageBody == null )
            {
                throw new InvalidOperationException( "Cannot create section match from a node without PageBody parent" );
            }

            return new SelectedNodeHandle()
                {
                    SelectedNode = attribute,
                    QueryMatchCreator = () => new QueryMatch( new Link( pageBody.Name.FullName, attribute.Value ) )
                };
        }
    }
}
