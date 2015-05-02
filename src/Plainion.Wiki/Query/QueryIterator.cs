using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.Utils;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary/>
    public class QueryIterator : IQueryIterator
    {
        // encapsulates member variables which work as cache for 
        // query functions
        private struct Cache
        {
            public bool? IsLinked;
            public bool? IsDefined;
        }

        private Cache myCache;

        /// <summary/>
        public QueryIterator( CompiledQuery query )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( "query" );
            }

            Query = query;
            myCache = new Cache();
        }

        /// <summary/>
        public CompiledQuery Query
        {
            get;
            private set;
        }

        /// <summary>
        /// The node of the AST the iterator is currently pointing too.
        /// </summary>
        public PageLeaf CurrentNode
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the value of the given attribute
        /// </summary>
        public string GetIdentifierValue( string identifer )
        {
            EnsureInitialized();

            var attr = CurrentNode as PageAttribute;
            if ( attr == null )
            {
                return null;
            }

            if ( attr.IsDefinition && attr.FullName == identifer )
            {
                return attr.Value;
            }

            return null;
        }

        private void EnsureInitialized()
        {
            if ( CurrentNode == null )
            {
                throw new Exception( "CurrentNode not set" );
            }
        }

        /// <summary>
        /// Returns true if current page is already linked on the page the query
        /// was defined on.
        /// <remarks>
        /// This is a function with page scope. Once it got true it will always stay 
        /// true for the lifetime of the iterator.
        /// </remarks>
        /// </summary>
        public bool Linked()
        {
            EnsureInitialized();

            return CachedFindOnCurrentPage( ref myCache.IsLinked, page =>
                {
                    var finder = new AstFinder<Link>( link => link.IsLinkingPage( page.Name ) );
                    var match = finder.FirstOrDefault( Query.GetParentOfType<PageBody>() );
                    return match != null;
                } );
        }

        private bool CachedFindOnCurrentPage( ref bool? cachedValue, Func<PageBody, bool> realGetter )
        {
            if ( cachedValue.HasValue )
            {
                return cachedValue.Value;
            }

            var pageBody = CurrentNode.GetParentOfType<PageBody>();
            if ( pageBody == null )
            {
                cachedValue = false;
            }
            else
            {
                cachedValue = realGetter( pageBody );
            }

            return cachedValue.Value;
        }

        /// <summary>
        /// Returns true if the given identifier is an attribute and it is 
        /// defined on the current page. 
        /// <remarks>
        /// This is a function with page scope. Once it got true it will always stay 
        /// true for the lifetime of the iterator.
        /// </remarks>
        /// </summary>
        public bool Defined( string attribute )
        {
            EnsureInitialized();

            return CachedFindOnCurrentPage( ref myCache.IsDefined, page =>
            {
                var finder = new AstFinder<PageAttribute>( attr => attr.IsDefinition && attr.FullName == attribute );
                var match = finder.FirstOrDefault( page );
                return match != null;
            } );
        }

        /// <summary>
        /// Returns true if the CurrentNode represents a PageAttribute definition
        /// of the given attribute fullname.
        /// </summary>
        public bool All( string attribute )
        {
            EnsureInitialized();

            var attr = CurrentNode as PageAttribute;
            if ( attr == null )
            {
                return false;
            }

            return attr.IsDefinition && attr.FullName == attribute;
        }
    }
}
