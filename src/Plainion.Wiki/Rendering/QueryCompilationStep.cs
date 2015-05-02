using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;
using System.Runtime.Remoting.Contexts;
using Plainion.Wiki.Query;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    [RenderingStep(RenderingStage.QueryCompilation)]
    public class QueryCompilationStep : IRenderingStep
    {
        /// <summary/>
        [ImportingConstructor]
        public QueryCompilationStep(QueryCompiler compiler)
        {
            if (compiler == null)
            {
                throw new ArgumentNullException("compiler");
            }

            Compiler = compiler;
        }

        /// <summary/>
        public QueryCompiler Compiler
        {
            get;
            private set;
        }

        /// <summary/>
        public PageLeaf Transform(PageLeaf node, EngineContext context)
        {
            var walker = new AstWalker<QueryDefinition>(CompileQuery);
            walker.Visit(node);

            return node;
        }

        private void CompileQuery(QueryDefinition query)
        {
            var compiledQuery = GetOrCompileQuery(query);
            query.Parent.ReplaceChild(query, compiledQuery);
        }

        private CompiledQuery GetOrCompileQuery(QueryDefinition query)
        {
            // Attention: compiled queries can only be cached if the context (e.g. the page) is taken into account
            // because the same QueryDefinition could result in different compiled queries for differnt pages 
            // (see NoParentFromClause)
            return Compiler.Compile(query);
        }
    }
}
