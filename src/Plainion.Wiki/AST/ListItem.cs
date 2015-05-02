using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents an item in a <see cref="BulletList"/>.
    /// An item can contain plain text and/or a inner list.
    /// </summary>
    [Serializable]
    public class ListItem : PageNode
    {
        /// <summary/>
        public ListItem( string text )
        {
            AddChild( new TextBlock( text ) );
        }

        /// <summary/>
        public ListItem( params PageLeaf[] children )
            : base( children )
        {
        }

        /// <summary>
        /// Text of the ListItem. Might be null.
        /// </summary>
        public TextBlock Text
        {
            get
            {
                return Children.OfType<TextBlock>().SingleOrDefault();
            }
        }

        /// <summary/>
        protected override bool CanConsume( PageLeaf part )
        {
            return part is TextBlock || part is BulletList;
        }

        /// <summary/>
        protected override void ConsumeInternal( PageLeaf part )
        {
            // all text will be in one TextBlock
            if ( part is TextBlock && Text != null )
            {
                Text.Consume( part );
            }
            else
            {
                AddChild( part );
            }
        }
    }
}
