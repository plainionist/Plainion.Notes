using System;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Defines an anchor inside a page which can be referenced by a <see cref="Link"/>
    /// </summary>
    [Serializable]
    public class Anchor : Markup
    {
        /// <summary/>
        public Anchor( string name )
        {
            Contract.RequiresNotNullNotWhitespace( name, "name" );

            Name = name;
        }

        /// <summary/>
        public string Name
        {
            get;
            private set;
        }
    }
}
