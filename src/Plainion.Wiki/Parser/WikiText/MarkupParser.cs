using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Collections;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Parser
{
    /// <summary>
    /// Scans a <see cref="PageBody"/> for markups and replaces them by
    /// AST elements.
    /// </summary>
    public class MarkupParser
    {
        private class Context
        {
            public TextBlock CurrentTextBlock;
            public Queue<PageLeaf> RemainingTextLeaves;
        }

        private Context myContext;
        private QueryParser myQueryParser;
        private WikiWords myWikiWords;

        /// <summary/>
        public MarkupParser()
            : this( new WikiWords() )
        {
        }

        /// <summary/>
        public MarkupParser( WikiWords wikiWords )
        {
            myQueryParser = new QueryParser();
            myWikiWords = wikiWords;
        }

        /// <summary>
        /// Parses the given page and replaces markups in-place.
        /// </summary>
        public void Parse( PageBody page )
        {
            myContext = new Context();

            var walker = new AstWalker<TextBlock>( Parse );
            walker.Visit( page );
        }

        private void Parse( TextBlock text )
        {
            myContext.CurrentTextBlock = text;
            myContext.RemainingTextLeaves = myContext.CurrentTextBlock.Children.ToQueue();
            myContext.CurrentTextBlock.RemoveAllChildren();

            while ( myContext.RemainingTextLeaves.Any() )
            {
                var textLeaf = myContext.RemainingTextLeaves.Dequeue();
                if ( textLeaf is PlainText )
                {
                    Parse( (PlainText)textLeaf );
                }
                else
                {
                    myContext.CurrentTextBlock.Consume( textLeaf );
                }
            }
        }

        private void Parse( PlainText text )
        {
            var tokenizer = new TextTokenizer( text.Text );
            var tokens = new Queue<string>( tokenizer.Tokens );

            while ( tokens.Count > 0 )
            {
                if ( ReadWhitespace( tokens ) )
                {
                    continue;
                }

                if ( ReadFreeLink( tokens ) )
                {
                    continue;
                }

                if ( ReadLinkClippedMarkups( tokens ) )
                {
                    continue;
                }

                if ( ReadMarkups( tokens ) )
                {
                    continue;
                }

                // finally just by-pass the token
                AddTextToCurrentTextBlock( tokens.Dequeue() );
            }
        }

        private bool ReadWhitespace( Queue<string> tokens )
        {
            var token = tokens.Peek();

            if ( !TextTokenizer.IsWhitespace( token ) )
            {
                return false;
            }

            AddTextToCurrentTextBlock( token );

            tokens.Dequeue();
            return true;
        }

        private void AddTextToCurrentTextBlock( string text )
        {
            var plainText = new PlainText( text );
            myContext.CurrentTextBlock.Consume( plainText );
        }

        // URLs and WikiWords
        private bool ReadFreeLink( Queue<string> tokens )
        {
            var token = tokens.Peek();

            if ( Link.IsExternalLink( token ) )
            {
                tokens.Dequeue();

                AddMarkupToCurrentTextBlock( new Link( token ) );

                return true;
            }

            var wikiWordLength = GetRealLengthOfWikiWord( token );
            var wikiWord = token.Substring( 0, wikiWordLength );

            if ( IsWikiWord( wikiWord ) )
            {
                tokens.Dequeue();

                AddMarkupToCurrentTextBlock( new Link( wikiWord ) );

                if ( wikiWordLength != token.Length )
                {
                    var tokenCutOff = token.Substring( wikiWordLength );
                    tokens.PushBack( tokenCutOff );

                    return false;
                }

                return true;
            }

            return false;
        }

        // HACK - cut off special chars at end of wiki workd
        private int GetRealLengthOfWikiWord( string token )
        {
            return token.ToCharArray()
                .TakeWhile( c => char.IsLetterOrDigit( c ) || c == '_' )
                .Count();
        }

        // read markups which are or look like link markups, e.g.:
        // - [url|text]
        // - [PageName], [PageName#<marker>] (page + anchor)
        // - [PageName|Alternative pagename]
        // - marker: [#todo]
        // - attributes: [@page.type: stock]
        private bool ReadLinkClippedMarkups( Queue<string> tokens )
        {
            var token = tokens.Peek();

            var markup = ReadClippedMarkup( tokens, "[", "]" );
            if ( markup == null )
            {
                return false;
            }

            if ( markup.StartsWith( "@" ) )
            {
                HandleAttribute( markup.Substring( 1 ) );

                return true;
            }

            if ( markup.StartsWith( "#" ) )
            {
                HandleAnchor( markup.Substring( 1 ) );

                return true;
            }

            var seperatorPos = markup.IndexOf( "|" );
            if ( seperatorPos < 0 )
            {
                AddMarkupToCurrentTextBlock( new Link( markup ) );
                return true;
            }

            var url = markup.Substring( 0, seperatorPos );
            var text = markup.Substring( seperatorPos + 1, markup.Length - seperatorPos - 1 );

            AddMarkupToCurrentTextBlock( new Link( url, text ) );

            return true;
        }

        private bool IsWikiWord( string word )
        {
            if ( myWikiWords.Contains( word ) )
            {
                return true;
            }

            if ( string.IsNullOrEmpty( word ) || !IsCapitalLetter( word[ 0 ] ) )
            {
                return false;
            }

            return word.ToCharArray()
                .SkipWhile( c => IsCapitalLetter( c ) )
                .SkipWhile( c => !IsCapitalLetter( c ) )
                .Any();
        }

        private static bool IsCapitalLetter( char c )
        {
            return char.IsLetter( c ) && char.IsUpper( c );
        }

        // [@page.name]
        // [@page.type: stock]
        private void HandleAttribute( string attribute )
        {
            var tokens = attribute.Split( ':' );
            if ( tokens.Length > 2 )
            {
                throw new ArgumentException( "invalid attribute syntax: " + attribute );
            }

            var qName = tokens[ 0 ];
            var value = tokens.Length > 1 ? tokens[ 1 ] : null;

            if ( qName == "query" )
            {
                var query = myQueryParser.Parse( value );
                AddMarkupToCurrentTextBlock( query );
                return;
            }

            tokens = qName.Split( '.' );
            if ( tokens.Length > 2 )
            {
                throw new ArgumentException( "invalid attribute syntax: " + attribute );
            }

            var type = tokens[ 0 ];
            var name = tokens.Length == 2 ? tokens[ 1 ] : null;

            AddMarkupToCurrentTextBlock( new PageAttribute( type, name, value ) );
        }

        private void HandleAnchor( string anchor )
        {
            AddMarkupToCurrentTextBlock( new Anchor( anchor ) );
        }

        private void AddMarkupToCurrentTextBlock( PageLeaf markup )
        {
            myContext.CurrentTextBlock.Consume( markup );
        }

        // markup which is identified by special start and stop chars
        private string ReadClippedMarkup( Queue<string> tokens, string beginChar, string endChar )
        {
            var token = tokens.Peek();

            if ( !token.StartsWith( beginChar ) )
            {
                return null;
            }

            // read till end to check for end of markup
            var markupTokens = tokens
                .TakeUntil( t => t.EndsWith( endChar ) )
                .ToList();

            if ( !markupTokens.Last().EndsWith( endChar ) )
            {
                // end of tokens reached but end of markup not found
                return null;
            }

            markupTokens.Count.Times( n => tokens.Dequeue() );

            var markup = string.Join( string.Empty, markupTokens.ToArray() );
            markup = markup.Substring( 1, markup.Length - 2 );

            return markup;
        }

        private bool ReadMarkups( Queue<string> tokens )
        {
            var token = tokens.Peek();

            var markup = ReadClippedMarkup( tokens, "{", "}" );
            if ( markup == null )
            {
                return false;
            }

            if ( markup.StartsWith( "h!" ) )
            {
                var content = markup.Substring( 2 );
                AddMarkupToCurrentTextBlock( new HighlightText( content, 1 ) );

                return true;
            }

            if ( markup.StartsWith( "hh!" ) )
            {
                var content = markup.Substring( 3 );
                AddMarkupToCurrentTextBlock( new HighlightText( content, 2 ) );

                return true;
            }

            return false;
        }
    }

    internal static class Extensions
    {
        public static IEnumerable<T> TakeUntil<T>( this IEnumerable<T> list, Func<T, bool> predicate )
        {
            foreach ( var item in list )
            {
                yield return item;

                if ( predicate( item ) )
                {
                    yield break;
                }
            }
        }
        /// <summary>
        /// Pushes the given element back to the front of the queue.
        /// </summary>
        public static void PushBack<T>( this Queue<T> source_in, T item )
        {
            var items = source_in.ToList();
            items.Insert( 0, item );

            source_in.Clear();

            foreach ( var x in items )
            {
                source_in.Enqueue( x );
            }
        }
    }
}
