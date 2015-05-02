using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Selects the related headline (section) of the matched node.
    /// Selects the whole page if no related headline could be found.
    /// </summary>
    public class SectionSelectClause : AbstractMultiSelectClause
    {
        /// <summary/>
        protected override SelectedNodeHandle Select( PageLeaf node )
        {
            var pageBody = node.GetParentOfType<PageBody>();
            if ( pageBody == null )
            {
                throw new InvalidOperationException( "Cannot create section match from a node without PageBody parent" );
            }

            var headline = node.FindRelatedHeadline();
            if ( headline == null )
            {
                return SelectedNodeHandle.CreatePageMatch( pageBody );
            }

            return new SelectedNodeHandle()
                {
                    SelectedNode = headline,
                    QueryMatchCreator = () => new QueryMatch( new Link(
                        pageBody.Name.FullName, headline.Anchor, headline.Text ) )
                };
        }
    }
}
