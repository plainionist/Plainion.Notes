using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Defines the body (content) of a page.
    /// </summary>
    [Serializable]
    public class PageBody : PageNode
    {
        /// <summary/>
        public PageBody( params PageLeaf[] children )
            : this( null, children )
        {
        }

        /// <summary/>
        public PageBody( PageName name )
            : this( name, new PageLeaf[] { } )
        {
        }

        /// <summary/>
        public PageBody( PageName name, params PageLeaf[] children )
            : base( children )
        {
            Name = name;
            Type = PageBodyType.Content;
        }

        /// <summary>
        /// Name of the page (content).
        /// Might be used by the renderer to define the name/title of the
        /// entire page.
        /// </summary>
        public PageName Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Default: <see cref="PageBodyType.Content"/>
        /// </summary>
        public PageBodyType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Can consume everything.
        /// </summary>
        protected override bool CanConsume( PageLeaf part )
        {
            return true;
        }

        /// <summary>
        /// Consumes the given part.
        /// A <see cref="PlainText"/> will always be wrapped by a <see cref="TextBlock"/>.
        /// A <see cref="TextBlock"/> will always be wrapped by a <see cref="Paragraph"/>.
        /// </summary>
        protected override void ConsumeInternal( PageLeaf part )
        {
            if ( part is PlainText )
            {
                var textBlock = new TextBlock();
                textBlock.Consume( part );

                part = textBlock;
            }

            // always start with paragraph for simple text
            if ( part is TextBlock )
            {
                var para = new Paragraph();
                para.Consume( part );

                part = para;
            }

            AddChild( part );
        }
    }
}
