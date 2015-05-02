using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.Rendering.PageAttributeTransformers;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Rendering
{
    [TestFixture]
    public class PageAttributeTransformerCatalogTests
    {
        [Test]
        public void Composition_WhenHappened_WikiDefaultStepsFound()
        {
            var catalog = new AssemblyCatalog( typeof( RenderingPipeline ).Assembly );
            var container = new CompositionContainer( catalog );
            container.ComposeParts( catalog );

            var expectedTransformers = new Dictionary<string, Type>();
            expectedTransformers[ "site.search" ] = typeof( SiteSearchTransformer );
            expectedTransformers[ "site.recentedits" ] = typeof( RecentEditsTransformer );
            expectedTransformers[ "page.reverselinks" ] = typeof( ReverseLinksTransformer );
            expectedTransformers[ "page.name" ] = typeof( PageNameTransformer );
            expectedTransformers[ "page.edit" ] = typeof( PageEditTransformer );
            XAssert.PluginCatalogContains( expectedTransformers, container.GetExportedValue<PageAttributeTransformerCatalog>() );
        }
    }
}
