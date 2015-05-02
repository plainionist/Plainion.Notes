using System.IO;
using Microsoft.Win32;

namespace Plainion.Httpd
{
    public class MimeTypes
    {
        public static string GetMimeTypeFromFile( string file )
        {
            try
            {
                string ext = Path.GetExtension( file );

                if ( string.IsNullOrEmpty( ext ) )
                {
                    return null;
                }

                // convert to lowercase as registry has lowercase keys
                ext = ext.ToLower();

                var registryKey = Registry.ClassesRoot.OpenSubKey( ext );

                if ( registryKey == null || registryKey.GetValue( "Content Type" ) == null )
                {
                    return null;
                }

                return (string)registryKey.GetValue( "Content Type" );
            }
            catch
            {
                return null;
            }
        }
    }
}
