using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// A <see cref="IPageDescriptor"/> describing an in-memory page. This class is 
    /// often used as transport container (e.g. to transport the new content for an 
    /// update an existing page).
    /// </summary>
    public class InMemoryPageDescriptor : IPageDescriptor
    {
        private List<string> myContent;

        /// <summary/>
        public InMemoryPageDescriptor( PageName name, params string[] content )
            : this( name, content.ToList() )
        {
        }

        /// <summary/>
        public InMemoryPageDescriptor( PageName name, IEnumerable<string> content )
        {
            if ( name == null )
            {
                throw new ArgumentNullException( "name" );
            }
            if ( content == null )
            {
                throw new ArgumentNullException( "content" );
            }

            Name = name;
            myContent = content.ToList();
        }

        /// <summary/>
        public PageName Name
        {
            get;
            private set;
        }

        /// <summary/>
        public void AddContent( params string[] content )
        {
            myContent.AddRange( content );
        }

        /// <summary/>
        public string[] GetContent()
        {
            return myContent.ToArray();
        }

        /// <summary/>
        public bool Matches( string searchText )
        {
            return searchText == null ||
                Name.Name.Contains( searchText, StringComparison.OrdinalIgnoreCase ) ||
                GetContent().Any( line => line.Contains( searchText, StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
