using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion;

namespace Plainion.Wiki.DataAccess
{
    [InheritedExport( typeof( IPageAccess ) )]
    public class PageAccessDecoratorBase : IPageAccess
    {
        private IPageAccess myPageAccess;

        [ImportingConstructor]
        public PageAccessDecoratorBase( IPageAccess pageAccess )
        {
            Contract.RequiresNotNull( pageAccess, "pageAccess" );

            myPageAccess = pageAccess;
        }

        /// <summary/>
        public virtual IEnumerable<IPageDescriptor> Pages
        {
            get { return myPageAccess.Pages; }
        }

        /// <summary/>
        public virtual IPageDescriptor Find( PageName pageName )
        {
            return myPageAccess.Find( pageName );
        }

        /// <summary/>
        public virtual void Create( IPageDescriptor pageDescriptor )
        {
            myPageAccess.Create( pageDescriptor );
        }

        /// <summary/>
        public virtual void Delete( IPageDescriptor pageDescriptor )
        {
            myPageAccess.Delete( pageDescriptor );
        }

        /// <summary/>
        public virtual void Update( IPageDescriptor pageDescriptor )
        {
            myPageAccess.Update( pageDescriptor );
        }

        /// <summary/>
        public virtual void Move( PageName pageName, PageNamespace newNamespace )
        {
            myPageAccess.Move( pageName, newNamespace );
        }
    }
}
