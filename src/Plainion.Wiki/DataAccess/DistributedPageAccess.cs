using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Provides access to a distributed page repository.
    /// <remarks>
    /// <para>
    /// A distributed page repository consists of multiple page repository
    /// each managed by its own <see cref="IPageAccess"/>. Each page repository
    /// (IPageAccess) will be registered for a specific namespace. On each page access the
    /// <see cref="PageName"/> will be matched against the registered page repositories
    /// using the namespace. If a PageName cannot be matched this request will be forwarded
    /// to "fallback"/"default" IPageAccess.
    /// </para>
    /// <para>
    /// The namespace of the PageName will be handled completely transparent to the 
    /// underlying "real" PageAccess. That means a PageName "rewriting" will take place.
    /// </para>
    /// </remarks>
    /// </summary>
    public class DistributedPageAccess : IPageAccess
    {
        private Dictionary<PageNamespace, IPageAccess> myNamespacePageAccessMap;

        /// <summary/>
        public DistributedPageAccess( IPageAccess defaultPageAccess )
        {
            if ( defaultPageAccess == null )
            {
                throw new ArgumentNullException( "defaultPageAccess" );
            }

            DefaultPageAccess = defaultPageAccess;

            myNamespacePageAccessMap = new Dictionary<PageNamespace, IPageAccess>();
        }

        /// <summary>
        /// Handles all page accesses which could not be dispatched to any other
        /// page access according to the PageName.
        /// </summary>
        public IPageAccess DefaultPageAccess
        {
            get;
            private set;
        }

        /// <summary/>
        public void Register( PageNamespace nspace, IPageAccess pageAccess )
        {
            Contract.RequiresNotNull( nspace, "nspace" );
            Contract.RequiresNotNull( pageAccess, "pageAccess" );
            Contract.Requires( !nspace.IsEmpty, "Empty namespace not allowed" );

            if ( myNamespacePageAccessMap.ContainsKey( nspace ) )
            {
                throw new ArgumentException( "A PageAccess for this namespace has already been registered" )
                    .AddContext( "Namespace", nspace );
            }

            myNamespacePageAccessMap[ nspace ] = pageAccess;
        }

        /// <summary/>
        public void Unregister( PageNamespace nspace )
        {
            if ( myNamespacePageAccessMap.ContainsKey( nspace ) )
            {
                myNamespacePageAccessMap.Remove( nspace );
            }
        }

        /// <summary/>
        public IEnumerable<KeyValuePair<PageNamespace, IPageAccess>> RegisteredPageAccesses
        {
            get
            {
                return myNamespacePageAccessMap;
            }
        }

        /// <summary/>
        public IEnumerable<IPageDescriptor> Pages
        {
            get
            {
                return DefaultPageAccess.Pages.Concat( GetPagesOfRegisteredPageAccesses() );
            }
        }

        private IEnumerable<IPageDescriptor> GetPagesOfRegisteredPageAccesses()
        {
            foreach ( var namespacePageAccessPair in myNamespacePageAccessMap )
            {
                foreach ( var pageDescriptor in namespacePageAccessPair.Value.Pages )
                {
                    yield return CreateAliasDescriptor( namespacePageAccessPair.Key, pageDescriptor );
                }
            }
        }

        private IPageDescriptor CreateAliasDescriptor( PageNamespace prefixNS, IPageDescriptor pageDescriptor )
        {
            var ns = prefixNS.Add( pageDescriptor.Name.Namespace );
            return new AliasPageDescriptor( PageName.Create( ns, pageDescriptor.Name.Name ), pageDescriptor );
        }

        /// <summary/>
        public IPageDescriptor Find( PageName pageName )
        {
            Contract.RequiresNotNull( pageName, "pageName" );

            var prefixedNamespace = GetPrefixedNamespace( pageName.Namespace );
            if ( prefixedNamespace == null )
            {
                return DefaultPageAccess.Find( pageName );
            }

            var registeredPageAccess = myNamespacePageAccessMap[ prefixedNamespace ];
            var pageNameWithoutPrefix = CreatePageNameWithoutPrefix( pageName, prefixedNamespace );

            var rawPage = registeredPageAccess.Find( pageNameWithoutPrefix );
            if ( rawPage == null )
            {
                return null;
            }

            return CreateAliasDescriptor( prefixedNamespace, rawPage );
        }

        private PageNamespace GetPrefixedNamespace( PageNamespace pageNamespace )
        {
            return myNamespacePageAccessMap.Keys
                .OrderByDescending( ns => ns.Elements.Length )
                .FirstOrDefault( ns => pageNamespace.StartsWith( ns ) );
        }

        private PageName CreatePageNameWithoutPrefix( PageName pageName, PageNamespace prefix )
        {
            var namespaceWithoutPrefix = pageName.Namespace.CutOffLeft( prefix );
            return PageName.Create( namespaceWithoutPrefix, pageName.Name );
        }

        /// <summary/>
        public void Create( IPageDescriptor pageDescriptor )
        {
            ForwardPageAccessAction( pageDescriptor, CreateForwardAction( "Create" ) );
        }

        private Action<IPageAccess, IPageDescriptor> CreateForwardAction( string actionName )
        {
            var mi = typeof( IPageAccess ).GetMethod( actionName );
            return ( pageAccess, pageDescriptor ) => mi.Invoke( pageAccess, new[] { pageDescriptor } );
        }

        private void ForwardPageAccessAction( IPageDescriptor pageDescriptor, Action<IPageAccess, IPageDescriptor> action )
        {
            if ( pageDescriptor == null )
            {
                throw new ArgumentNullException( "pageDescriptor" );
            }

            var prefixedNamespace = GetPrefixedNamespace( pageDescriptor.Name.Namespace );
            if ( prefixedNamespace == null )
            {
                action( DefaultPageAccess, pageDescriptor );
                return;
            }

            var registeredPageAccess = myNamespacePageAccessMap[ prefixedNamespace ];
            var pageNameWithoutPrefix = CreatePageNameWithoutPrefix( pageDescriptor.Name, prefixedNamespace );

            action( registeredPageAccess, new AliasPageDescriptor( pageNameWithoutPrefix, pageDescriptor ) );
        }

        /// <summary/>
        public void Delete( IPageDescriptor pageDescriptor )
        {
            ForwardPageAccessAction( pageDescriptor, CreateForwardAction( "Delete" ) );
        }

        /// <summary/>
        public void Update( IPageDescriptor pageDescriptor )
        {
            ForwardPageAccessAction( pageDescriptor, CreateForwardAction( "Update" ) );
        }

        /// <summary/>
        public void Move( PageName pageName, PageNamespace newNamespace )
        {
            var prefixedNamespace = GetPrefixedNamespace( pageName.Namespace );
            if ( prefixedNamespace == null )
            {
                DefaultPageAccess.Move( pageName, newNamespace );
                return;
            }

            var registeredPageAccess = myNamespacePageAccessMap[ prefixedNamespace ];
            var newNamespaceWithoutPrefix = newNamespace.CutOffLeft( prefixedNamespace );

            registeredPageAccess.Move( pageName, newNamespaceWithoutPrefix );
        }
    }
}
