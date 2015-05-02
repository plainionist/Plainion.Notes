using System;

namespace Plainion.Wiki.AST
{
    [Serializable]
    public class PreformattedText : Markup
    {
        public PreformattedText()
            : this( string.Empty )
        {
        }

        public PreformattedText( string text )
        {
            Text = text;
        }

        public string Text
        {
            get;
            private set;
        }

        public void Consume( string text )
        {
            Text += text;
        }
    }
}
