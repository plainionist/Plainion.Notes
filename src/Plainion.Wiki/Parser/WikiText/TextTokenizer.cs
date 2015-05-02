using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion;

namespace Plainion.Wiki.Parser
{
    internal class TextTokenizer
    {
        private string myText;

        public TextTokenizer( string text )
        {
            myText = text;
        }

        public IEnumerable<string> Tokens
        {
            get
            {
                return Split();
            }
        }

        public static bool IsWhitespace( string token )
        {
            return string.IsNullOrWhiteSpace( token );
        }

        private IEnumerable<string> Split()
        {
            var token = new StringBuilder();

            foreach ( var c in myText.ToCharArray() )
            {
                if ( IsNewTokenType( token, c ) )
                {
                    yield return token.ToString();

                    token = new StringBuilder();
                }

                token.Append( c );
            }

            if ( token.Length > 0 )
            {
                yield return token.ToString();
            }
        }

        private bool IsNewTokenType( StringBuilder token, char c )
        {
            if ( token.Length == 0 )
            {
                return false;
            }

            // hack for closing markups
            if ( token[ token.Length - 1 ] == ']' )
            {
                return true;
            }

            if ( char.IsWhiteSpace( c ) == char.IsWhiteSpace( token[ 0 ] ) )
            {
                return false;
            }

            //if ( char.IsLetterOrDigit( c ) && char.IsLetterOrDigit( token[ 0 ] ) )
            //{
            //    return false;
            //}

            return true;
        }
    }
}
