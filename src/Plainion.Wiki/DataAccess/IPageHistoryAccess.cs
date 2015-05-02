using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Provides access to the history of a page.
    /// </summary>
    public interface IPageHistoryAccess
    {
        /// <summary/>
        void CreateNewVersion( IPageDescriptor pageDescriptor );
    }
}
