using System;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Html.AST
{
    /// <summary>
    /// Block of plain Html.
    /// Will not be touched by parser and renderer.
    /// </summary>
    [Serializable]
    public class HtmlBlock : PageLeaf
    {
        private StringBuilder myStringBuilder;

        /// <summary/>
        public HtmlBlock( params string[] lines )
        {
            myStringBuilder = new StringBuilder();

            foreach ( var line in lines )
            {
                AppendLine( line );
            }
        }

        /// <summary/>
        public void AppendLine( string line )
        {
            myStringBuilder.AppendLine( line );
        }

        /// <summary/>
        public string Html
        {
            get { return myStringBuilder.ToString(); }
        }
    }
}
