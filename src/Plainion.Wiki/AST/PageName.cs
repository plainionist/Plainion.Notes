using System;
using System.Linq;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Defines the name of a page including its namespace.
    /// </summary>
    [Serializable]
    public class PageName
    {
        /// <summary/>
        protected PageName( PageNamespace pageNamespace, string name )
        {
            Contract.RequiresNotNullNotWhitespace( name, "name" );
            Contract.RequiresNotNull( pageNamespace, "pageNamespace" );

            Name = name;
            Namespace = pageNamespace;

            FullName = ( Namespace.IsEmpty ? string.Empty : Namespace.AsPath ) + "/" + Name;
        }

        /// <summary/>
        public string Name
        {
            get;
            private set;
        }

        /// <summary/>
        public PageNamespace Namespace
        {
            get;
            private set;
        }

        /// <summary/>
        public string FullName
        {
            get;
            private set;
        }

        /// <summary/>
        public override bool Equals( object obj )
        {
            if ( object.ReferenceEquals( this, obj ) )
            {
                return true;
            }

            var otherPageName = obj as PageName;
            if ( object.ReferenceEquals( otherPageName, null ) )
            {
                return false;
            }

            return FullName.Equals( otherPageName.FullName, StringComparison.OrdinalIgnoreCase );
        }

        /// <summary/>
        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        /// <summary/>
        public override string ToString()
        {
            return FullName;
        }

        /// <summary/>
        public static bool operator ==( PageName lhs, PageName rhs )
        {
            if ( object.ReferenceEquals( lhs, rhs ) )
            {
                return true;
            }
            if ( object.ReferenceEquals( lhs, null ) )
            {
                return false;
            }

            return lhs.Equals( rhs );
        }

        /// <summary/>
        public static bool operator !=( PageName lhs, PageName rhs )
        {
            return !( lhs == rhs );
        }

        /// <summary/>
        public static PageName Create( PageNamespace pageNamespace, string name )
        {
            return new PageName( pageNamespace, name );
        }

        /// <summary/>
        public static PageName Create( string name )
        {
            return new PageName( PageNamespace.Create(), name );
        }

        /// <summary>
        /// Creates a PageName from the given path.
        /// The parts of the path are separated by "/". The last part will become 
        /// the page name.
        /// </summary>
        public static PageName CreateFromPath( string path )
        {
            Contract.RequiresNotNullNotWhitespace( path, "path" );

            var tokens = path.Split( new[] { "/" }, StringSplitOptions.RemoveEmptyEntries );
            var ns = PageNamespace.Create( tokens.Length > 1 ? tokens.Take( tokens.Length - 1 ).ToArray() : null );
            var name = tokens.Last();

            return new PageName( ns, name );
        }
    }
}
