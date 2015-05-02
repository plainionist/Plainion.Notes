using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Query;
using Moq;

namespace Plainion.Wiki.UnitTests.Testing
{
    public static class FakeFactory2
    {
        public static CompiledQuery CreateCompiledQuery( Func<PageLeaf, bool> whereClause )
        {
            return CreateCompiledQuery( whereClause, new NodeSelectClause() );
        }

        public static CompiledQuery CreateCompiledQuery( Func<PageLeaf, bool> whereClause, ISelectClause selectClause )
        {
            var def = new QueryDefinition( "ignored" );

            var fromClause = new Mock<IFromClause> { DefaultValue = DefaultValue.Mock };

            var query = new CompiledQuery( def,
                new DynamicLinqWhereClause( whereClause ),
                selectClause,
                fromClause.Object );

            fromClause.Setup( x => x.IsQueryFromPageAllowed( It.IsAny<PageBody>() ) ).Returns( true );

            return query;
        }

        public static PageRepository CreateRepository( params string[] namesOfThePages )
        {
            var descriptors = namesOfThePages
                .Select( page => new InMemoryPageDescriptor( PageName.Create( page ), page ) )
                .OfType<IPageDescriptor>()
                .ToList();

            var pageAccess = new Mock<IPageAccess>();
            pageAccess.SetupGet( x => x.Pages ).Returns( descriptors );

            return new PageRepository( pageAccess.Object, new ParserPipeline() );
        }
    }
}
