using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Describes transformation of a page.
    /// </summary>
    public interface IPageTransformer
    {
        /// <summary/>
        IPageDescriptor Transform( IPageDescriptor pageDescriptor );
    }
}
