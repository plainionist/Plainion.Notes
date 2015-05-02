using System;
using System.Linq;
using NUnit.Framework;
using Plainion;

namespace Plainion.Testing
{
    public class BAssert
    {
        /// <summary>
        /// Asserts that both text are semantically equal. 
        /// <remarks>
        /// Removes all empty lines and white spaces and then compares
        /// the resulting text.
        /// </remarks>
        /// </summary>
        public static void TextSemanticallyEquals( string[] expected_in, string[] actual_in )
        {
            // remove empty lines
            var expected = expected_in.Where( line => !string.IsNullOrWhiteSpace( line ) ).ToArray();
            var actual = actual_in.Where( line => !string.IsNullOrWhiteSpace( line ) ).ToArray();

            // dont check length at the beginning - lets see how far we
            // can get to give as describtive as possible error message

            int minLines = Math.Min( expected.Length, actual.Length );
            for ( int i = 0; i < minLines; ++i )
            {
                var expectedLine = expected[ i ].RemoveAll( char.IsWhiteSpace );
                var actualLine = actual[ i ].RemoveAll( char.IsWhiteSpace );

                Assert.That( actualLine, Is.EqualTo( expectedLine ) );
            }

            Assert.That( expected.Length, Is.EqualTo( minLines ), "Expected has more lines than actual" );
            Assert.That( actual.Length, Is.EqualTo( minLines ), "Actual has more lines than expected" );
        }
    }
}
