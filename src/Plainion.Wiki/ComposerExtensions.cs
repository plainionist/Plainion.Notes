using System.Linq;
using System.Reflection;
using Plainion.Wiki.Rendering;
using Plainion.Composition;

namespace Plainion.Wiki
{
    public static class ComposerExtensions
    {
        public static void RegisterRenderingSteps( this IComposer self, Assembly assembly )
        {
            var defaultRenderActions = assembly.GetTypes()
                .Where( t => t.GetCustomAttributes( typeof( RenderingStepAttribute ), true ).Any() )
                .ToArray();

            self.Register( defaultRenderActions );
        }

        public static void RegisterPageAttributeTransformers( this IComposer self, Assembly assembly )
        {
            var defaultRenderActions = assembly.GetTypes()
                .Where( t => t.GetCustomAttributes( typeof( PageAttributeTransformerAttribute ), true ).Any() )
                .ToArray();

            self.Register( defaultRenderActions );
        }
    }
}
