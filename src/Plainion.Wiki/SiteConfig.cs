using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Plainion.Validation;
using System.Windows;

namespace Plainion.Wiki
{
    /// <summary>
    /// Simple wrapper around XML config for easy access.
    /// </summary>
    public class SiteConfig : DataTemplate
    {
        public SiteConfig()
        {
            HomePageName = "HomePage";
            RenderPageNameAsHeadline = true;
            ComponentConfigs = new Dictionary<string, object>();
        }

        [ValidateObject]
        public Dictionary<string, object> ComponentConfigs
        {
            get;
            private set;
        }

        public T GetComponentConfig<T>( string component ) where T : new()
        {
            if ( !ComponentConfigs.ContainsKey( component ) )
            {
                ComponentConfigs[ component ] = new T();
            }

            return (T)ComponentConfigs[ component ];
        }

        public string HomePageName
        {
            get;
            set;
        }

        public bool RenderPageNameAsHeadline
        {
            get;
            set;
        }

        public string StaticPageTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Name of default page used if a pure page namespace is requested (e.g. linked).
        /// </summary>
        public string NamespaceDefaultPageName
        {
            get;
            set;
        }
    }
}
