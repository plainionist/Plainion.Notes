using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki
{
    /// <summary>
    /// Engine of running Wiki site. It encapsulates the site, the parser and the 
    /// renderer and provides the main entry point for an application which 
    /// embeds Wiki or for an Http handler (<see cref="T:Plainion.Wiki.Http.BasicHttpController"/>).
    /// </summary>
    [Export( typeof( IEngine ) )]
    public class Engine : IEngine
    {
        private PageRepository myPageRepository;
        private QueryEngine myQueryEngine;

        [ImportingConstructor]
        public Engine( PageRepository repository )
        {
            if( repository == null )
            {
                throw new ArgumentNullException( "pageAccess" );
            }

            myPageRepository = repository;
            myQueryEngine = new QueryEngine( myPageRepository );
        }

        [Import]
        public RenderingPipeline RenderingPipeline
        {
            get;
            set;
        }

        [Import]
        public IErrorPageHandler ErrorPageHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Generic config which can be provided by the site.
        /// Structure: under root every class can its own config block.
        /// Inside this block the structure is class dependent.
        /// </summary>
        [Import( CompositionContractNames.SiteConfig )]
        public SiteConfig Config
        {
            get;
            set;
        }

        [Import]
        public IAuditingLog AuditingLog
        {
            get;
            set;
        }

        public void Render( PageName pageName, Stream output )
        {
            var pageDescriptor = myPageRepository.Find( pageName );
            if( pageDescriptor == null )
            {
                pageDescriptor = ErrorPageHandler.CreatePageNotFoundPage( pageName );
            }

            Render( pageDescriptor, output );
        }

        public void Render( IPageDescriptor pageDescriptor, Stream output )
        {
            Render( myPageRepository.Get( pageDescriptor ), output );
        }

        public void Render( PageBody pageBody, Stream output )
        {
            var finder = new AstFinder<PageAttribute>( attr => attr.Type.Equals( "redirect", StringComparison.OrdinalIgnoreCase ) );
            var redirect = finder.FirstOrDefault( pageBody );

            if( redirect != null )
            {
                var redirectPage = FindPageByName( pageBody.Name.Namespace, redirect.Value );
                if( redirectPage != null )
                {
                    pageBody = Get( redirectPage );
                }
            }

            using( var ctx = new RenderingContext( output ) )
            {
                ctx.EngineContext = new EngineContext();
                ctx.EngineContext.Query = myQueryEngine;
                ctx.EngineContext.Config = Config;
                ctx.EngineContext.AuditingLog = AuditingLog;
                ctx.EngineContext.PageExists = pageName => myPageRepository.Find( pageName ) != null;
                ctx.EngineContext.GetPage = pageName => myPageRepository.Get( pageName );
                ctx.EngineContext.FindPageByName = ( ns, name ) => FindPageByName( ns, name );

                RenderingPipeline.Render( pageBody, ctx );
            }
        }

        public PageName FindPageByName( PageNamespace ns, string name )
        {
            // support links to namespaces
            if( name.EndsWith( "/" ) )
            {
                name += Config.NamespaceDefaultPageName;
            }

            // pages relative to the current namespace always preceed other pages
            if( ns != null )
            {
                var pageName = PageName.Create( ns, name );
                if( Find( pageName ) != null )
                {
                    return pageName;
                }
            }

            // check link to absolute pages
            {
                var pageName = PageName.CreateFromPath( name );
                if( Find( pageName ) != null )
                {
                    return pageName;
                }
            }

            return null;
        }

        public IPageDescriptor Find( PageName pageName )
        {
            return myPageRepository.Find( pageName );
        }

        public PageBody Get( PageName pageName )
        {
            return myPageRepository.Get( pageName );
        }

        public void Create( PageName pageName, IEnumerable<string> pageContent )
        {
            myPageRepository.Create( pageName, pageContent );
        }

        public void Delete( PageName pageName )
        {
            myPageRepository.Delete( pageName );
        }

        public void Update( PageName pageName, IEnumerable<string> pageContent )
        {
            myPageRepository.Update( pageName, pageContent );
        }

        public QueryEngine Query
        {
            get
            {
                return myQueryEngine;
            }
        }

        public void Move( PageName pageName, PageNamespace newNamespace )
        {
            myPageRepository.Move( pageName, newNamespace );
        }
    }
}
