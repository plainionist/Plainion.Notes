using System.Linq;
using System.Reflection;
using Plainion.Wiki.Html.Rendering;
using Plainion.Composition;

namespace Plainion.Wiki.Html
{
    public static class ComposerExtensions
    {
        public static void RegisterRenderActions( this IComposer self, Assembly assembly )
        {
            var defaultRenderActions = assembly.GetTypes()
                .Where( t => t.GetCustomAttributes( typeof( HtmlRenderActionAttribute ), true ).Any() )
                .ToArray();

            self.Register( defaultRenderActions );
        }
    }
}
