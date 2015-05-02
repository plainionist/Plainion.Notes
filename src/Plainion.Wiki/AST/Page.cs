
using System;
namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Defines one page in the system (body + layout info)
    /// </summary>
    [Serializable]
    public class Page : PageNode
    {
        private PageBody myContent;
        private PageBody myHeader;
        private PageBody myFooter;
        private PageBody mySideBar;

        /// <summary/>
        public Page( PageName name )
        {
            if ( name == null )
            {
                throw new ArgumentNullException( "name" );
            }

            Name = name;
        }

        /// <summary/>
        public PageName Name
        {
            get;
            private set;
        }

        /// <summary/>
        public PageBody Content
        {
            get { return myContent; }
            set { SetPageBody( ref myContent, value ); }
        }

        private void SetPageBody( ref PageBody referencedPageBody, PageBody newPageBody )
        {
            if ( referencedPageBody != null )
            {
                RemoveChild( referencedPageBody );
            }

            referencedPageBody = newPageBody;

            if ( newPageBody != null )
            {
                AddChild( newPageBody );
            }
        }

        /// <summary/>
        public PageBody Header
        {
            get { return myHeader; }
            set { SetPageBody( ref myHeader, value ); }
        }

        /// <summary/>
        public PageBody Footer
        {
            get { return myFooter; }
            set { SetPageBody( ref myFooter, value ); }
        }

        /// <summary/>
        public PageBody SideBar
        {
            get { return mySideBar; }
            set { SetPageBody( ref mySideBar, value ); }
        }
    }
}
