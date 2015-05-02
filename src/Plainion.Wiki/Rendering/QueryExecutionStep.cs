using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using System.Runtime.Remoting.Contexts;
using Plainion.Wiki.Query;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    [RenderingStep( RenderingStage.QueryExecution )]
    public class QueryExecutionStep : IRenderingStep
    {
        /// <summary/>
        public PageLeaf Transform( PageLeaf node, EngineContext context )
        {
            var walker = new AstWalker<CompiledQuery>( query => Visit( query, context ) );
            walker.Visit( node );

            return node;
        }

        private void Visit( CompiledQuery query, EngineContext context )
        {
            var matcher = new DynamicExpressionMatcher( query );
            var hits = context.Query.Where( matcher );

            var content = ContentBuilder.BuildQueryResult( hits, "not results" );

            query.Parent.ReplaceChild( query, content );
        }
    }
}
