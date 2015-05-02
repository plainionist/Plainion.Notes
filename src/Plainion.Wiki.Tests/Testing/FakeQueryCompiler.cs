using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using Moq;

namespace Plainion.Wiki.UnitTests.Testing
{
    public class FakeQueryCompiler : QueryCompiler
    {
        protected override IFromClause CreateFromClause( QueryDefinition query )
        {
            return new Mock<IFromClause> { DefaultValue = DefaultValue.Mock }.Object;
        }

        protected override ISelectClause CreateSelector( QueryDefinition query )
        {
            return new Mock<ISelectClause> { DefaultValue = DefaultValue.Mock }.Object;
        }

        protected override IWhereClause CreateWhereClause( QueryDefinition query )
        {
            return new Mock<IWhereClause> { DefaultValue = DefaultValue.Mock }.Object;
        }
    }
}
