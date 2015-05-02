using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Plainion;

namespace Plainion.Testing
{
    public static class ObjectExtensions
    {        
        /// <summary>
        /// Generic hashcode generation for object graphs using serializers.
        /// Useful for simple before and after checks.
        /// </summary>
        public static int GetDeepHashCode( this object source )
        {
            if ( !source.GetType().IsSerializable )
            {
                throw new ArgumentException( "The type must be serializable.", "source" );
            }

            // Don't serialize a null object, simply return the default for that object
            if ( Object.ReferenceEquals( source, null ) )
            {
                return 0;
            }

            var formatter = new BinaryFormatter();
            using ( var stream = new MemoryStream() )
            {
                formatter.Serialize( stream, source );

                return Encoding.Unicode.GetString( stream.ToArray() ).GetHashCode();
            }
        }
    }
}
