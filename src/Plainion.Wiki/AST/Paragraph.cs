using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Collections;

namespace Plainion.Wiki.AST
{
    [Serializable]
    public class Paragraph : PageNode
    {
        private TextBlock myTextBlock;

        public Paragraph( params PageLeaf[] children )
        {
            myTextBlock = new TextBlock();

            AddChild( myTextBlock );

            if( children != null )
            {
                foreach( var child in children )
                {
                    myTextBlock.Consume( child );
                }
            }
        }

        public TextBlock Text
        {
            get
            {
                return myTextBlock;
            }
        }

        protected override bool CanConsume( PageLeaf part )
        {
            return part is TextBlock || part is PreformattedText;
        }

        protected override void ConsumeInternal( PageLeaf part )
        {
            myTextBlock.Consume( part );
        }
    }
}
