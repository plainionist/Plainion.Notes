using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion;
using Plainion.Wiki.AST;
using System.Text.RegularExpressions;

namespace Plainion.Wiki.Parser
{
    /// <summary>
    /// Parses the given content into a list.
    /// Multi-line for list items supported. Takes care of bullet indention and creates nested lists.
    /// </summary>
    public class ListParser
    {
        private static Regex myListPattern = new Regex( @"^(\s*)[-\*#]\s+(.*)$" );
        private PageBody myPage;
        private Context myContext;

        private struct Context
        {
            public Context( PageBody page )
            {
                RootList = new BulletList();
                CurrentList = RootList;
                CurrentItem = null;
                CurrentIndent = null;
            }

            public BulletList RootList;
            public BulletList CurrentList;
            public ListItem CurrentItem;

            // will be set by the first bullet
            public int? CurrentIndent;
        }

        /// <summary/>
        public ListParser( PageBody page )
        {
            myPage = page;
        }

        /// <summary/>
        public static bool IsListStart( string line )
        {
            return myListPattern.IsMatch( line );
        }

        /// <summary/>
        public static bool IsPotentialLineItem( string line )
        {
            return myListPattern.IsMatch( line ) || IsMultiLineItem( line );
        }

        /// <summary/>
        private static bool IsMultiLineItem( string line )
        {
            return line.StartsWith( " " ) || line.StartsWith( "\t" );
        }

        /// <summary>
        /// see overload.
        /// </summary>
        public void Parse( params string[] content )
        {
            Parse( content as IEnumerable<string> );
        }

        /// <summary>
        /// Parses the content.
        /// Expects the complete content to be part of a list.
        /// </summary>
        public void Parse( IEnumerable<string> content )
        {
            myContext = new Context( myPage );

            foreach ( var line in content )
            {
                if ( ReadNewListItem( line ) )
                {
                    continue;
                }

                if ( ReadMultiLineListItem( line ) )
                {
                    continue;
                }

                throw new Exception( "Cannot handle line: " + line );
            }

            myPage.Consume( myContext.RootList );
        }

        private bool ReadNewListItem( string line )
        {
            var md = myListPattern.Match( line );
            if ( !md.Success )
            {
                return false;
            }

            var indent = md.Groups[ 1 ].Value.Length;
            var text = md.Groups[ 2 ].Value;

            HandleIndention( indent );

            myContext.CurrentItem = new ListItem();
            AddTextToCurrentItem( text );

            if ( !myContext.CurrentList.Items.Any() )
            {
                myContext.CurrentList.Ordered = line.TrimStart()[ 0 ] == '#';
            }

            myContext.CurrentList.Consume( myContext.CurrentItem );

            return true;
        }

        private void HandleIndention( int indent )
        {
            if ( myContext.CurrentIndent == null )
            {
                myContext.CurrentIndent = indent;
            }

            if ( indent > myContext.CurrentIndent )
            {
                // new sub-list
                myContext.CurrentList = new BulletList();
                myContext.CurrentItem.Consume( myContext.CurrentList );
            }
            else if ( indent < myContext.CurrentIndent )
            {
                // back to outer list
                myContext.CurrentList = (BulletList)myContext.CurrentList.Parent.Parent;
            }

            // independent from condition above new currentIndent is always this one
            myContext.CurrentIndent = indent;
        }

        private void AddTextToCurrentItem( string text )
        {
            var textBlock = new TextBlock( text );
            myContext.CurrentItem.Consume( textBlock );
        }

        private bool ReadMultiLineListItem( string line )
        {
            if ( !IsMultiLineItem( line ) )
            {
                return false;
            }

            AddTextToCurrentItem( line.Trim() );

            return true;
        }
    }
}
