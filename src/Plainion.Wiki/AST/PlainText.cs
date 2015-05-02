using System;

namespace Plainion.Wiki.AST
{
    [Serializable]
    public class PlainText : PageLeaf
    {
        public PlainText()
            : this( string.Empty )
        {
        }

        public PlainText( string text )
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
