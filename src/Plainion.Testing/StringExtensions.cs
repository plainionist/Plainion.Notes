using System.Collections.Generic;
using System.IO;

namespace Plainion.Testing
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns an iterator to all lines contained in string. New line separator is Environment.NewLine.
        /// </summary>
        public static IEnumerable<string> AsLines( this string str )
        {
            var reader = new StringReader( str );

            var line = reader.ReadLine();
            while ( line != null )
            {
                yield return line;
                line = reader.ReadLine();
            }
        }
    }
}
