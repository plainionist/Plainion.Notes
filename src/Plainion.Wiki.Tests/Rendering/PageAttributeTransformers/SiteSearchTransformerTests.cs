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
    public class SiteSearchTransformerTests
    {
        [Test]
        public void Transform_WhenCalled_AttributeReplacedByPageName()
        {
            var attr = new PageAttribute( "site", "searcg" );
            var page = new PageBody( PageName.Create( "a" ), attr );
            var transformer = new SiteSearchTransformer();

            transformer.Transform( attr, new EngineContext() );

            var searchForm = page.Children.Single();
            Assert.That( searchForm, Is.InstanceOf<SiteSearchForm>() );
        }
    }
}
