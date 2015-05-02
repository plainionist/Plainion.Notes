using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents a list of <see cref="ListItem"/>s.
    /// </summary>
    [Serializable]
    public class BulletList : PageNode
    {
        /// <summary/>
        public BulletList( params ListItem[] items )
            : base( items )
        {
        }

        /// <summary/>
        public bool Ordered
        {
            get;
            set;
        }

        /// <summary/>
        protected override bool CanConsume( PageLeaf part )
        {
            return part is ListItem || part is BulletList;
        }

        /// <summary/>
        protected override void ConsumeInternal( PageLeaf part )
        {
            AddChild( part );
        }

        /// <summary/>
        public IEnumerable<ListItem> Items
        {
            get { return Children.OfType<ListItem>(); }
        }
    }
}
