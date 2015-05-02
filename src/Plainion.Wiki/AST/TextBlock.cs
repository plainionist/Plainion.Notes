using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Block of text containing plaintext and markups.
    /// </summary>
    [Serializable]
    public class TextBlock : PageNode
    {
        public TextBlock( string text )
        {
            AddChild( new PlainText( text ) );
        }

        public TextBlock( params PageLeaf[] children )
            : base( children )
        {
        }

        protected override bool CanConsume( PageLeaf part )
        {
            return part is TextBlock || part is PlainText || part is Markup;
        }

        /// <summary>
        /// TextBlocks will be merged and separated by new lines.
        /// PlainText elements will be merged without separator.
        /// </summary>
        protected override void ConsumeInternal( PageLeaf part )
        {
            if( part is TextBlock )
            {
                if( Children.Any() )
                {
                    // interpret new textblock as new line
                    Consume( new PlainText( Environment.NewLine ) );
                }

                foreach( var leaf in ( ( TextBlock )part ).Children )
                {
                    Consume( leaf );
                }

                return;
            }

            if( part is PreformattedText )
            {
                var lastPreformattedText = Children.LastOrDefault() as PreformattedText;

                if( lastPreformattedText != null )
                {
                    lastPreformattedText.Consume( ( ( PreformattedText )part ).Text );

                    return;
                }
            }

            if( part is PlainText )
            {
                var lastPlainText = Children.LastOrDefault() as PlainText;

                if( lastPlainText != null )
                {
                    lastPlainText.Consume( ( ( PlainText )part ).Text );

                    return;
                }
            }

            AddChild( part );
        }
    }
}
