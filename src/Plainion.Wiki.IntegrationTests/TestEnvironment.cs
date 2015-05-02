using System;
using System.Linq;
using System.IO;
using System.Text;

namespace Plainion.Wiki.IntegrationTests
{
    public class TestEnvironment
    {
        public TestEnvironment( Type testFixture )
        {
            TestFixture = testFixture;
            TestDataRoot = new Lazy<string>( GetTestDataRoot );
        }

        public Type TestFixture
        {
            get;
            private set;
        }

        private string GetTestDataRoot()
        {
            //Plainion.Tests
            //Plainion.Wiki.IntegrationTests
            //Plainion.Httpd.Tests.Rrp
            var name = TestFixture.Namespace;

            // ignore root
            var tokens = name.Split( '.' )
                .Skip( 1 )
                .ToList();

            var productNamespace = new StringBuilder();
            var testLevel = string.Empty;
            foreach( var token in tokens )
            {
                if( token == "Tests" || token == "IntegrationTests" )
                {
                    testLevel = token;
                }
                else
                {
                    productNamespace.Append( token );
                    if( token != tokens.Last() )
                    {
                        productNamespace.Append( "." );
                    }
                }
            }


            return Path.Combine( Path.GetDirectoryName( TestFixture.GetType().Assembly.Location ), "TestData", testLevel, productNamespace.ToString() );
        }

        public static Lazy<string> TestDataRoot
        {
            get;
            private set;
        }

        public string GetTestResource( string name )
        {
            return Path.Combine( TestDataRoot.Value, name );
        }
    }
}
