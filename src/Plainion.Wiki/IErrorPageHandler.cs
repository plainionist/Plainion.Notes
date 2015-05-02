using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;

namespace Plainion.Wiki
{
    /// <summary>
    /// Provides access to error pages.
    /// </summary>
    public interface IErrorPageHandler
    {
        /// <summary/>
        IPageDescriptor CreatePageNotFoundPage( PageName missingPage );

        /// <summary/>
        IPageDescriptor CreateGeneralErrorPage( string error );
    }
}
