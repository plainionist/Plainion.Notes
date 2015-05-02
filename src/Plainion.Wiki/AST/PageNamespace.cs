using System;
using System.Collections.Generic;
using System.Linq;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Defines the namespace of a page.
    /// </summary>
    [Serializable]
    public class PageNamespace
    {
        private static string[] EmptyElements = new string[] { };

        /// <summary/>
        public static PageNamespace Empty = PageNamespace.Create();

        /// <summary/>
        protected PageNamespace( IEnumerable<string> elements )
        {
            Elements = elements == null ? EmptyElements : elements.ToArray();
            AsPath = "/" + string.Join( "/", Elements );
        }

        /// <summary/>
        public string[] Elements
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the namespace as path.
        /// The elements are joined by slashes.
        /// If the namespace is empty it returns a single slash.
        /// </summary>
        public string AsPath
        {
            get;
            private set;
        }

        /// <summary/>
        public bool IsEmpty
        {
            get { return Elements.Length == 0; }
        }

        /// <summary/>
        public override bool Equals( object obj )
        {
            if ( object.ReferenceEquals( this, obj ) )
            {
                return true;
            }

            var otherPageNamespace = obj as PageNamespace;
            if ( object.ReferenceEquals( otherPageNamespace, null ) )
            {
                return false;
            }

            return AsPath.Equals( otherPageNamespace.AsPath );
        }

        /// <summary/>
        public override int GetHashCode()
        {
            return AsPath.GetHashCode();
        }

        /// <summary/>
        public override string ToString()
        {
            return AsPath;
        }

        /// <summary/>
        public static bool operator ==( PageNamespace lhs, PageNamespace rhs )
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
        public static bool operator !=( PageNamespace lhs, PageNamespace rhs )
        {
            return !( lhs == rhs );
        }

        /// <summary/>
        public static PageNamespace Create( params string[] path )
        {
            return new PageNamespace( path );
        }

        /// <summary>
        /// Creates a PageNamespace from the given path.
        /// The elements of the path are separated by "/".
        /// </summary>
        public static PageNamespace Create( string path )
        {
            if ( path == null )
            {
                return Create();
            }

            var elements = path.Trim().Split( new[] { "/" }, StringSplitOptions.RemoveEmptyEntries );

            return Create( elements );
        }

        /// <summary/>
        public bool StartsWith( PageNamespace ns )
        {
            if ( Elements.Length < ns.Elements.Length )
            {
                return false;
            }

            var potentialPrefix = Elements.Take( ns.Elements.Length ).ToArray();

            return potentialPrefix.SequenceEqual( ns.Elements );
        }

        /// <summary/>
        public PageNamespace CutOffLeft( PageNamespace ns )
        {
            if ( !StartsWith( ns ) )
            {
                throw new ArgumentException( "Namespace does not start with the given one" )
                    .AddContext( "This namespace", this )
                    .AddContext( "Given namespace", ns );
            }

            return new PageNamespace( Elements.Skip( ns.Elements.Length ) );
        }

        /// <summary/>
        public PageNamespace Add( PageNamespace subNs )
        {
            var allElements = Elements.Concat( subNs.Elements ).ToArray();
            return PageNamespace.Create( allElements );
        }
    }
}
