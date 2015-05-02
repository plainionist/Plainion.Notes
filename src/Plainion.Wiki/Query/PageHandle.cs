using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki.Query
{
    /// <summary>
    /// Internal page handle used by <see cref="QueryEngine"/> to group a <see cref="IPageDescriptor"/>
    /// and a parsed <see cref="PageBody"/> into one object.
    /// </summary>
    public class PageHandle
    {
        /// <summary/>
        public PageHandle( IPageDescriptor descriptor, PageBody body )
        {
            if ( descriptor == null )
            {
                throw new ArgumentNullException( "descriptor" );
            }
            if ( body == null )
            {
                throw new ArgumentNullException( "body" );
            }

            Descriptor = descriptor;
            Body = body;
        }

        /// <summary/>
        public PageName Name
        {
            get { return Descriptor.Name; }
        }

        /// <summary/>
        public IPageDescriptor Descriptor
        {
            get;
            private set;
        }

        /// <summary/>
        public PageBody Body
        {
            get;
            private set;
        }
    }
}
