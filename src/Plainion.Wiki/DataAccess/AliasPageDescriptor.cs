using System;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Wraps an existing <see cref="IPageDescriptor"/> to provide a different
    /// name to the same content.
    /// </summary>
    public class AliasPageDescriptor : IPageDescriptor
    {
        /// <summary/>
        public AliasPageDescriptor( PageName name, IPageDescriptor originalPageDescriptor )
        {
            if ( name == null )
            {
                throw new ArgumentNullException( "name" );
            }
            if ( originalPageDescriptor == null )
            {
                throw new ArgumentNullException( "originalPageDescriptor" );
            }

            Name = name;
            OriginalPageDescriptor = originalPageDescriptor;
        }

        /// <summary/>
        public PageName Name
        {
            get;
            private set;
        }

        /// <summary/>
        public IPageDescriptor OriginalPageDescriptor
        {
            get;
            private set;
        }

        /// <summary/>
        public string[] GetContent()
        {
            return OriginalPageDescriptor.GetContent();
        }

        /// <summary/>
        public bool Matches( string searchText )
        {
            return Name.Name.Contains( searchText, StringComparison.OrdinalIgnoreCase ) ||
                OriginalPageDescriptor.Matches( searchText );
        }
    }
}
