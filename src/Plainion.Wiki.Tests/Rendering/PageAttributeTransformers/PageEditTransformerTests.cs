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
    public class PageEditTransformerTests
    {
        [Test]
        public void Transform_WhenCalled_AttributeReplacedByEditLink()
        {
            var attr = new PageAttribute( "page", "edit" );
            var page = new PageBody( PageName.Create( "a" ), attr );
            var transformer = new PageEditTransformer();

            transformer.Transform( attr, new EngineContext() );

            var link = page.Children.OfType<Link>().SingleOrDefault();
            var expectedLink = new Link( page.Name.FullName + "?action=edit", "Edit" );
            expectedLink.IsStatic = true;
            XAssert.LinkEquals( expectedLink, link );
        }

        [Test]
        public void Transform_WithPageAsRoot_NameOfPageIsUsed()
        {
            var attr = new PageAttribute( "page", "edit" );
            var page = new Page( PageName.Create( "a" ) );
            page.Content = new PageBody( PageName.Create( "x" ), attr );
            var transformer = new PageEditTransformer();

            transformer.Transform( attr, new EngineContext() );

            var link = page.Content.Children.OfType<Link>().SingleOrDefault();
            var expectedLink = new Link( page.Name.FullName + "?action=edit", "Edit" );
            expectedLink.IsStatic = true;
            XAssert.LinkEquals( expectedLink, link );
        }
    }
}
