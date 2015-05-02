using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Query;
using Moq;

namespace Plainion.Wiki.UnitTests.Query
{
    /// <summary/>
    public static class MockFactory
    {
        /// <summary>
        /// Creates a compiled query from the given expression with mocked clauses.
        /// </summary>
        public static CompiledQuery CreateCompiledQuery( string expression )
        {
            var def = new QueryDefinition( expression );

            return new CompiledQuery( def,
                new Mock<IWhereClause> { DefaultValue = DefaultValue.Mock}.Object,
                new Mock<ISelectClause> { DefaultValue = DefaultValue.Mock }.Object,
                new Mock<IFromClause> { DefaultValue = DefaultValue.Mock }.Object );
        }

        /// <summary/>
        public static PageHandle CreatePage( PageName pageName, params string[] pageContent )
        {
            var descriptor = new InMemoryPageDescriptor( pageName, pageContent );
            return new PageHandle( descriptor, new PageBody( descriptor.Name ) );
        }
    }
}
