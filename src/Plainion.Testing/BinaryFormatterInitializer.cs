using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Plainion.Testing
{
    public class BinaryFormatterInitializer
    {
        static BinaryFormatterInitializer()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        // http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/e5f0c371-b900-41d8-9a5b-1052739f2521
        // BinaryFormatter does not check whether required assembly is already loaded - i uses usual loading mechanism.
        // This causes exceptions if executed in NUnit runner started from installation root instead of "bin"
        static Assembly CurrentDomain_AssemblyResolve( object sender, ResolveEventArgs args )
        {
            Assembly ayResult = null;

            string sShortAssemblyName = args.Name.Split( ',' )[ 0 ];

            Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach ( Assembly ayAssembly in ayAssemblies )
            {
                if ( sShortAssemblyName == ayAssembly.FullName.Split( ',' )[ 0 ] )
                {
                    ayResult = ayAssembly;
                    break;
                }
            }

            return ayResult;
        }
    }
}
