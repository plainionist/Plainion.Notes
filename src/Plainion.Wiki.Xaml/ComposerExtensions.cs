using System.Linq;
using System.Reflection;
using Plainion.Wiki.Xaml.Rendering;
using Plainion.Composition;

namespace Plainion.Wiki.Xaml
{
    public static class ComposerExtensions
    {
        public static void RegisterRenderActions( this IComposer self, Assembly assembly )
        {
            var defaultRenderActions = assembly.GetTypes()
                .Where( t => t.GetCustomAttributes( typeof( XamlRenderActionAttribute ), true ).Any() )
                .ToArray();

            self.Register( defaultRenderActions );
        }
    }
}
