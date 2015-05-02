using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// NullObject for <see cref="IPageTransformer"/>.
    /// </summary>
    public class NullPageTransformer : IPageTransformer
    {
        /// <summary/>
        public IPageDescriptor Transform( IPageDescriptor pageDescriptor )
        {
            return pageDescriptor;
        }
    }
}
