using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Plainion.Testing
{
    public static class MemoryStreamExtensions
    {
        public static string[] GetLines( this MemoryStream self )
        {
            return self.ReadLines().ToArray();
        }

        public static IEnumerable<string> ReadLines( this MemoryStream self )
        {
            // XXX: i am not sure whether this is correct - check with 
            // DataModelSerializer
            var buffer = Encoding.UTF8.GetString( self.ToArray() );

            using ( var reader = new StringReader( buffer ) )
            {
                while ( reader.Peek() > 0 )
                {
                    yield return reader.ReadLine();
                }
            }
        }
    }
}
