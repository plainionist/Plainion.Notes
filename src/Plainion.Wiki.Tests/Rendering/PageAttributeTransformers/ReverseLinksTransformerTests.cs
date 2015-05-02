using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering.PageAttributeTransformers;
using NUnit.Framework;
using Plainion.Wiki.UnitTests.Testing;

namespace Plainion.Wiki.UnitTests.Rendering.PageAttributeTransformers
{
    [TestFixture]
    public class ReverseLinksTransformerTests
    {
        [Test]
        public void Transform_WithoutPageBody_Throws()
        {
            var attr = new PageAttribute( "page", "reverselinks" );
            var transformer = new ReverseLinksTransformer();

            Assert.Throws<InvalidOperationException>(
                () => transformer.Transform( attr, new EngineContext() ) );
        }

        [Test]
        public void Transform_WhenCalled_AttributeReplacedByContent()
        {
            var attr = new PageAttribute( "page", "reverselinks" );
            var page = new PageBody( PageName.Create( "a" ), attr );
            var transformer = new ReverseLinksTransformer();

            var ctx = new EngineContext();
            ctx.Query = new QueryEngine( FakeFactory2.CreateRepository( "p1", "p2" ) );
            transformer.Transform( attr, ctx );

            Assert.That( page.Children.Single(), Is.InstanceOf<Content>() );
        }
    }
}
