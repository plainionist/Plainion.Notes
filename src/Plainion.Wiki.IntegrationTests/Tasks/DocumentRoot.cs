using System;
using System.ComponentModel.Composition;
using System.Threading;
using Plainion.IO;
using Plainion.Testing;
using Plainion.Wiki.Resources;
using System.Reflection;
using System.Collections.Generic;
using Plainion;
using System.IO;

namespace Plainion.Wiki.IntegrationTests.Tasks
{
    public class DocumentRoot
    {
        public DocumentRoot( IFileSystem fs )
        {
            Directory = fs.GetTempPath().Directory( "Wiki.DocRoot" );
        }

        public IDirectory Directory
        {
            get;
            private set;
        }

        public void Create()
        {
            Directory.Create();
            Console.WriteLine( "DocumentRoot: {0}", Directory.Path );
        }

        public void Delete()
        {
            if( Directory.Exists )
            {
                Thread.Sleep( 5 );
                Directory.Delete( true );
            }
        }

        public void InitializeWithDefaults()
        {
            DeployResource( ResourceNames.CssStylesheet );
            DeployResource( ResourceNames.JavaScript );
        }

        public void DeployResource( string resource )
        {
            var output = Directory.File( resource );
            CopyEmbeddedTextResource( typeof( IEngine ).Assembly, typeof( IEngine ).Namespace + ".Resources." + resource, output );
        }

        public static void CopyEmbeddedTextResource( Assembly self, string resource, IFile output )
        {
            using( var writer = output.CreateWriter() )
            {
                var stream = self.GetManifestResourceStream( resource );
                Contract.Invariant( stream != null, "Could not get resource: " + resource );
                using( var reader = new StreamReader( stream ) )
                {
                    while( !reader.EndOfStream )
                    {
                        writer.WriteLine( reader.ReadLine() );
                    }
                }
            }
        }

        public string[] GetResource( string resource )
        {
            return GetEmbeddedTextResource( typeof( IEngine ).Assembly,
                typeof( IEngine ).Namespace + ".Resources." + resource );
        }

        public static string[] GetEmbeddedTextResource( Assembly self, string resource )
        {
            var lines = new List<string>();

            var stream = self.GetManifestResourceStream( resource );
            Contract.Invariant( stream != null, "Could not get resource: " + resource );
            using( var reader = new StreamReader( stream ) )
            {
                while( !reader.EndOfStream )
                {
                    lines.Add( reader.ReadLine() );
                }
            }

            return lines.ToArray();
        }
    }
}
