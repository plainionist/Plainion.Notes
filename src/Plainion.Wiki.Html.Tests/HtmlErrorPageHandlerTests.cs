using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html;

namespace Plainion.Wiki.UnitTests
{
    [TestFixture]
    public class DefaultErrorPageHandlerTests
    {
        [Test]
        public void CreatePageNotFoundPage_WhenCalled_PageIsReturned()
        {
            var handler = new DefaultErrorPageHandler();

            var page = handler.CreatePageNotFoundPage( PageName.Create( "a" ) );

            Assert.That( page.Name.Name, Is.EqualTo( "PageNotFound" ) );
        }

        [Test]
        public void CreateGeneralErrorPage_WhenCalled_PageIsReturned()
        {
            var handler = new DefaultErrorPageHandler();

            var page = handler.CreateGeneralErrorPage( "error" );

            Assert.That( page.Name.Name, Is.EqualTo( "GeneralError" ) );
        }
    }
}
