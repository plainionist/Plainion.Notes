using System;
using System.Collections.Generic;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Html.Rendering.RenderActions;
using Plainion.Wiki.Rendering;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering
{
    [TestFixture]
    public class HtmlRendererTest : TestBase
    {
        private class TestableRenderer : HtmlRenderer
        {
            public TestableRenderer( HtmlRenderActionCatalog catalog )
                : base( catalog )
            {
            }

            public IDictionary<Type, IRenderAction> RenderingActions
            {
                get
                {
                    return RenderActions;
                }
            }
        }

        [Test]
        public void Ctor_WhenCalled_RenderActionsAreAvailable()
        {
            var catalog = new HtmlRenderActionCatalog();
            catalog.Plugins[ typeof( Anchor ) ] = new AnchorRenderAction();

            var renderer = new TestableRenderer( catalog );

            var renderActionPair = renderer.RenderingActions.Single();
            Assert.That( renderActionPair.Key, Is.EqualTo( typeof( Anchor ) ) );
            Assert.That( renderActionPair.Value, Is.EqualTo( catalog.Plugins[ typeof( Anchor ) ] ) );
        }

        [Test]
        public void GetStylesheet_WhenCalled_WillNeverReturnNull()
        {
            var catalog = new HtmlRenderActionCatalog();
            var renderer = new TestableRenderer( catalog );

            Assert.That( renderer.Stylesheet, Is.Not.Null );

            renderer.Stylesheet = null;

            Assert.That( renderer.Stylesheet, Is.Not.Null );
        }
    }
}
