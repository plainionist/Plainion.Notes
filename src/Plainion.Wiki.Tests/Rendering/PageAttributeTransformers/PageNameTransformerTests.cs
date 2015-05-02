using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering.PageAttributeTransformers;
using Plainion.Wiki.UnitTests.Testing;

namespace Plainion.Wiki.UnitTests.Rendering.PageAttributeTransformers
{
    [TestFixture]
    public class PageNameTransformerTests
    {
        [Test]
        public void Transform_WhenCalled_AttributeReplacedByPageName()
        {
            var attr = new PageAttribute( "page", "name" );
            var page = new PageBody( PageName.Create( "a" ), attr );
            var transformer = new PageNameTransformer();

            transformer.Transform( attr, new EngineContext() );

            var text = page.Children.OfType<PlainText>().SingleOrDefault();
            Assert.That( text.Text, Is.EqualTo( "a" ) );
        }

        [Test]
        public void Transform_WithPageAsRoot_NameOfPageIsUsed()
        {
            var attr = new PageAttribute( "page", "name" );
            var page = new Page( PageName.Create( "a" ) );
            page.Content = new PageBody( PageName.Create( "x" ), attr );
            var transformer = new PageNameTransformer();

            transformer.Transform( attr, new EngineContext() );

            var text = page.Content.Children.OfType<PlainText>().SingleOrDefault();
            Assert.That( text.Text, Is.EqualTo( "a" ) );
        }
    }
}
