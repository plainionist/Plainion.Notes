using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Plainion.Wiki.AST;
using Plainion;

namespace Plainion.Wiki.Parser
{
    /// <summary/>
    public class StructureParser
    {
        private static Regex myHeadlinePattern = new Regex( @"^(!+)\s(.*)$" );
        private static Regex myListPattern = new Regex( @"^(\s*)[-\*]\s+(.*)$" );

        private struct Context
        {
            public PageBody Page;
        }

        private Context myContext;

        /// <summary/>
        public PageBody Parse( PageName name, string[] content )
        {
            myContext = new Context();

            myContext.Page = new PageBody( name );

            Parse( content );

            return myContext.Page;
        }

        private void Parse( string[] pageContent )
        {
            var content = new Queue<string>( pageContent );

            while( content.Count > 0 )
            {
                if( string.IsNullOrWhiteSpace( content.Peek() ) )
                {
                    // ignore empty lines
                    content.Dequeue();
                    continue;
                }

                if( ReadHeader( content ) )
                {
                    continue;
                }

                if( ReadList( content ) )
                {
                    continue;
                }

                if( ReadParagraph( content ) )
                {
                    continue;
                }
            }
        }

        private bool ReadHeader( Queue<string> content )
        {
            var md = myHeadlinePattern.Match( content.Peek() );
            if( !md.Success )
            {
                return false;
            }

            var size = md.Groups[ 1 ].Value.Length;
            var text = md.Groups[ 2 ].Value;

            var headline = new Headline( text, size );

            myContext.Page.Consume( headline );

            content.Dequeue();
            return true;
        }

        private bool ReadList( Queue<string> content )
        {
            if( !ListParser.IsListStart( content.Peek() ) )
            {
                return false;
            }

            var listContent = content
                .TakeWhile( line => !string.IsNullOrWhiteSpace( line ) )
                .TakeWhile( line => ListParser.IsPotentialLineItem( line ) )
                .ToList();

            listContent.Count.Times( n => content.Dequeue() );

            var listParser = new ListParser( myContext.Page );
            listParser.Parse( listContent );

            return true;
        }

        // just reads from non-empty line to next empty line.
        // does not take care about lists
        private bool ReadParagraph( Queue<string> content )
        {
            var para = new Paragraph();

            while( content.Count > 0 && !string.IsNullOrWhiteSpace( content.Peek() ) )
            {
                var text = content.Dequeue();
                if( text.StartsWith( "  " ) )
                {
                    var element = new PreformattedText( text.Substring( 2 ) );
                    element.Consume( Environment.NewLine );
                    para.Consume( element );
                }
                else
                {
                    para.Consume( new TextBlock( text ) );
                }
            }

            myContext.Page.Consume( para );
            return true;
        }
    }
}
