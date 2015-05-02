using System;

namespace Plainion.Wiki.AST
{
    [Serializable]
    public class HighlightText : Markup
    {
        public HighlightText( string content, int level )
        {
            Contract.RequiresNotNullNotWhitespace( content, "content" );

            Content = content;
            Level = level;
        }

        public string Content
        {
            get;
            private set;
        }

        public int Level
        {
            get;
            private set;
        }
    }
}
