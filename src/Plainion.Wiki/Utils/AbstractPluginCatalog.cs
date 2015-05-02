using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Utils
{
    /// <summary>
    /// Generic catalog of MEF plugins. Plugins are cataloged by keys which were extracted
    /// from their metadata. It supports overriding mechanism for plugins if the same plugin
    /// exists in multiple assemblies.
    /// </summary>
    public abstract class AbstractPluginCatalog<TKey, TValue, TMetadata> : IPartImportsSatisfiedNotification, IPluginCatalog<TKey, TValue>
    {
#pragma warning disable 649
        // row "imports" from MEF
        [ImportMany]
        private IEnumerable<Lazy<TValue, TMetadata>> myImports;
#pragma warning restore 649

        /// <summary/>
        protected AbstractPluginCatalog()
        {
            // allow catalog to be used without MEF where plugins are added manually
            Plugins = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Access to the raw "imports" from MEF. Allows derived classes to
        /// specify different MEF "import".
        /// </summary>
        protected virtual IEnumerable<Lazy<TValue, TMetadata>> Imports
        {
            get { return myImports; }
        }

        /// <summary/>
        public IDictionary<TKey, TValue> Plugins
        {
            get;
            private set;
        }

        /// <summary>
        /// Called by MEF after all "imports" has been satisfied.
        /// Overriding mechanism will now be applied and the plugins will be
        /// cataloged by their keys.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if ( Imports == null || !Imports.Any() )
            {
                return;
            }

            // to allow overwriting we have to handle potential Wiki defaults first
            var sortedImports = Imports
                .OrderBy( pair => GetOverrideOrder( pair.Value ) );

            foreach ( var import in sortedImports )
            {
                Plugins[ GetKey( import.Metadata ) ] = import.Value;
            }
        }

        /// <summary>
        /// Allows the derived class to implement a different "overriding" policy.
        /// Default: plugins from default Wiki assembly will always be overwritten from
        /// user assemblies.
        /// </summary>
        protected virtual int GetOverrideOrder( TValue plugin )
        {
            return plugin.GetType().Assembly == GetType().Assembly ? 1 : 2;
        }

        /// <summary>
        /// Defines how to extract the key from the plugins metadata.
        /// </summary>
        protected abstract TKey GetKey( TMetadata metadata );
    }
}
