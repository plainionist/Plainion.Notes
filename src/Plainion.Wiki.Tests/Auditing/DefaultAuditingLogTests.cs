using System;
using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Auditing
{
    [TestFixture]
    public class DefaultAuditingTests
    {
        [Test]
        public void Log_WithNullAction_Throws()
        {
            var log = new DefaultAuditingLog();

            Assert.Throws<ArgumentNullException>( () => log.Log( null ) );
        }

        [Test]
        public void Log_WhenCalled_ActionGetsLogged()
        {
            var log = new DefaultAuditingLog();
            var action = new CreateAction( PageName.Create( "a" ) );

            log.Log( action );

            var loggedAction = log.Actions.Single();
            Assert.That( loggedAction, Is.SameAs( action ) );
        }
    }
}
