using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml.Linq;

namespace Plainion.Wiki.UnitTests
{
    [TestFixture]
    public class SiteConfigTests
    {
        [Test]
        public void Ctor_WhenCalled_HomePageNameIsSet()
        {
            var config = new SiteConfig();

            Assert.That( config.HomePageName, Is.EqualTo( "HomePage" ) );
        }
    }
}
