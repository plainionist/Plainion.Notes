using System;
using Plainion.Wiki.AST;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Query
{
    [Export( typeof( QueryCompiler ) )]
    public class QueryCompiler
    {
        /// <summary/>
        public CompiledQuery Compile( QueryDefinition query )
        {
            return new CompiledQuery( query,
                CreateWhereClause( query ),
                CreateSelector( query ),
                CreateFromClause( query ) );
        }

        /// <summary>
        /// Null or empty strings as where clause will be interpreted as: "always false".
        /// </summary>
        protected virtual IWhereClause CreateWhereClause( QueryDefinition query )
        {
            return new DynamicLinqWhereClause( query.WhereExpression );
        }

        /// <summary/>
        protected virtual ISelectClause CreateSelector( QueryDefinition query )
        {
            if ( query.SelectExpression == "section" )
            {
                return new SectionSelectClause();
            }
            if ( query.SelectExpression == "parent" )
            {
                return new ParentSelectClause();
            }
            if ( query.SelectExpression == "attribute-value" )
            {
                return new AttributeValueSelectClause();
            }

            return new PageSelectClause();
        }

        /// <summary/>
        protected virtual IFromClause CreateFromClause( QueryDefinition query )
        {
            if ( query.FromExpression == "down-only" )
            {
                var pageBody = query.GetParentOfType<PageBody>();
                if ( pageBody == null )
                {
                    throw new InvalidOperationException( "Cannot define NoParentFromClause from query without PageBody parent" );
                }

                return new NoParentFromClause( pageBody.Name );
            }

            return new AlwaysTrueFromClause();
        }
    }
}
