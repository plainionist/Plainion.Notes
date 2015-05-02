using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.AST;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.UnitTests.Utils
{
    [TestFixture]
    public class AbstractPluginCatalogTests
    {
        public class TestableCatalog : AbstractPluginCatalog<string, PageLeaf, string>
        {
            private IList<Lazy<PageLeaf, string>> myImports = new List<Lazy<PageLeaf, string>>();

            protected override IEnumerable<Lazy<PageLeaf, string>> Imports
            {
                get { return myImports; }
            }

            public void AddImport( PageLeaf value, string metadata )
            {
                myImports.Add( new Lazy<PageLeaf, string>( () => value, metadata ) );
            }

            protected override string GetKey( string metadata )
            {
                return metadata;
            }

            public Func<PageLeaf, int> OverridePolicy { get; set; }

            protected override int GetOverrideOrder( PageLeaf plugin )
            {
                return OverridePolicy == null ? base.GetOverrideOrder( plugin ) : OverridePolicy( plugin );
            }
        }

        [Test]
        public void OnImportsSatisfied_NoImports_PluginsEmpty()
        {
            var catalog = new TestableCatalog();

            catalog.OnImportsSatisfied();

            Assert.That( catalog.Plugins, Is.Empty );
        }

        [Test]
        public void OnImportsSatisfied_WithImports_PluginsShouldBeFilled()
        {
            var plugin1 = new PlainText();
            var plugin2 = new LineBreak();
            var catalog = new TestableCatalog();
            catalog.AddImport( plugin1, "a" );
            catalog.AddImport( plugin2, "b" );

            catalog.OnImportsSatisfied();

            var expectedPlugins = new Dictionary<string, PageLeaf>()
            {
                { "a", plugin1 },
                { "b", plugin2 },
            };
            Assert.That( catalog.Plugins, Is.EquivalentTo( expectedPlugins ) );
        }

        [Test]
        public void OnImportsSatisfied_WithOverridingImports_OverridingPolicyApplies()
        {
            var plugin = new LineBreak();
            var catalog = new TestableCatalog();
            catalog.AddImport( plugin, "a" );
            catalog.AddImport( new PlainText(), "a" );
            catalog.OverridePolicy = p => p is LineBreak ? int.MaxValue : 0;

            catalog.OnImportsSatisfied();

            var expectedPlugins = new Dictionary<string, PageLeaf>()
            {
                { "a", plugin },
            };
            Assert.That( catalog.Plugins, Is.EquivalentTo( expectedPlugins ) );
        }
    }
}
