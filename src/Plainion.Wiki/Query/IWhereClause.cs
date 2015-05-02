using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Selects matching nodes on a page.
    /// </summary>
    public interface IWhereClause
    {
        /// <summary/>
        IQueryIterator CreateIterator( CompiledQuery query );

        /// <summary/>
        bool Where( IQueryIterator iterator );
    }
}
