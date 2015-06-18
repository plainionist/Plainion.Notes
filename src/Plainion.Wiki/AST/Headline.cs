using System;
using System.Text;
using Plainion;
using System.Linq;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Represents a headline inside a page.
    /// </summary>
    [Serializable]
    public class Headline : PageLeaf
    {
        public Headline( string text, int fontSize )
        {
            Text = text;
            Size = fontSize;
            Anchor = Text;
        }
        
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// Size of the headline. 1 is biggest. The bigger the number the smaller the headline
        /// </summary>
        public int Size
        {
            get;
            private set;
        }

        public string Anchor
        {
            get;
            set;
        }
    }
}
