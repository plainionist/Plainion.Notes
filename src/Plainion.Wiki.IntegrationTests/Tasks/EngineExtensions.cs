using System.IO;
using Plainion.Wiki.AST;
using Plainion.Testing;

namespace Plainion.Wiki.IntegrationTests.Tasks
{
    public static class EngineExtensions
    {
        public static string[] Render(this IEngine self, PageName pageName)
        {
            using ( var memStream = new MemoryStream() )
            {
                self.Render( pageName, memStream );

                return memStream.GetLines();
            }
        }

        public static PageName CreatePage(this IEngine self, string pagePath, params string[] content)
        {
            var pageName = PageName.CreateFromPath( pagePath );
            self.Create( pageName, content );

            return pageName;
        }
    }
}
