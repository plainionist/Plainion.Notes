using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.Query;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Provides access to the raw pages of a site.
    /// </summary>
    public interface IPageAccess
    {
        /// <summary/>
        IEnumerable<IPageDescriptor> Pages { get; }

        /// <summary/>
        IPageDescriptor Find( PageName pageName );

        /// <summary/>
        void Create( IPageDescriptor pageDescriptor );

        /// <summary/>
        void Delete( IPageDescriptor pageDescriptor );

        /// <summary/>
        void Update( IPageDescriptor pageDescriptor );

        /// <summary>
        /// Moves the given page to the new namespace. References to this page will not be updated.
        /// </summary>
        void Move( PageName pageName, PageNamespace newNamespace );
    }
}
