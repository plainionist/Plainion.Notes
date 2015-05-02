using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.Utils
{
    /// <summary>
    /// Defines a catalog for plugins in Wiki.
    /// </summary>
    public interface IPluginCatalog<TKey, TValue>
    {
        /// <summary/>
        IDictionary<TKey, TValue> Plugins
        {
            get;
        }
    }
}
