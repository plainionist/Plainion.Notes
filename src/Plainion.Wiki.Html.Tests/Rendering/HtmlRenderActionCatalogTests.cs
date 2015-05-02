using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Html.Rendering.RenderActions;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering
{
    [TestFixture]
    public class HtmlRenderActionCatalogTests
    {
        [Test]
        public void Composition_WhenHappened_WikiDefaultStepsFound()
        {
            var catalog = new AggregateCatalog(
                new AssemblyCatalog( typeof( RenderingPipeline ).Assembly ),
                new AssemblyCatalog( typeof( AnchorRenderAction ).Assembly ) );
            var container = new CompositionContainer( catalog );
            container.ComposeParts( catalog );

            var expectedActions = new Dictionary<Type, Type>();
            expectedActions[ typeof( Anchor ) ] = typeof( AnchorRenderAction );
            expectedActions[ typeof( Link ) ] = typeof( LinkRenderAction );
            expectedActions[ typeof( BulletList ) ] = typeof( BulletListRenderAction );

            XAssert.PluginCatalogContains( expectedActions, container.GetExportedValue<HtmlRenderActionCatalog>() );
        }
    }
}
