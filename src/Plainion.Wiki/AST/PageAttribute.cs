using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents a generic page attribute.
    /// This node just stores the content of a page attribute definition or
    /// attribute reference without knowledge about the semantics.
    /// </summary>
    [Serializable]
    public class PageAttribute : Markup
    {
        /// <summary/>
        public PageAttribute( string type, string name )
            : this( type, name, null )
        {
        }

        /// <summary/>
        public PageAttribute( string type, string name, string value )
        {
            if ( type == null )
            {
                throw new ArgumentNullException( "type" );
            }

            Type = type.Trim();
            Name = name != null ? name.Trim() : null;
            Value = value != null ? value.Trim() : null;

            if ( Name != null )
            {
                FullName = string.Format( "{0}.{1}", Type, Name );
            }
            else
            {
                FullName = Type;
            }
        }

        /// <summary>
        /// Fully qualified name including type and name.
        /// </summary>
        public string FullName
        {
            get;
            private set;
        }

        /// <summary/>
        public string Type
        {
            get;
            private set;
        }

        /// <summary/>
        public string Name
        {
            get;
            private set;
        }

        /// <summary/>
        public string Value
        {
            get;
            private set;
        }

        /// <summary/>
        public bool IsDefinition
        {
            get
            {
                return Value != null;
            }
        }
    }
}
