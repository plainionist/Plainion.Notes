using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.Rendering.PageAttributeTransformers;
using Plainion.Wiki.UnitTests.Testing;
using Plainion.Wiki.Utils;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests.Rendering.PageAttributeTransformers
{
    [TestFixture]
    public class RecentEditsTransformerTests
    {
        [Test]
        public void Transform_NoAuditLog_TransformsToNotApplicable()
        {
            var attr = new PageAttribute( "site", "recentedits" );
            var page = new PageBody( PageName.Create( "a" ), attr );
            var transformer = new RecentEditsTransformer();

            transformer.Transform( attr, new EngineContext() );

            var text = page.GetFlattenedTree().OfType<PlainText>().SingleOrDefault();
            Assert.That( text.Text, Is.EqualTo( "n.a." ) );
        }

        [Test]
        public void Transform_WhenCalled_AttributeReplacedRecentEdits()
        {
            var attr = new PageAttribute( "site", "recentedits" );
            var page = new PageBody( PageName.Create( "a" ), attr );
            var transformer = new RecentEditsTransformer();
            var ctx = new EngineContext();
            var auditingLog = new Mock<IAuditingLog> { DefaultValue = DefaultValue.Mock };
            ctx.AuditingLog = auditingLog.Object;
            var actions = new IAuditingAction[] { 
                new CreateAction( PageName.Create("p6") ),
                new CreateAction( PageName.Create("p5") ),
                new UpdateAction( PageName.Create("p3") ),
                new UpdateAction( PageName.Create("p4") ),
                new CreateAction( PageName.Create("p3") ),
                new CreateAction( PageName.Create("p2") ),
                new UpdateAction( PageName.Create("p1") ),
            };
            auditingLog.SetupGet( x => x.Actions ).Returns( actions );

            transformer.Transform( attr, ctx );

            var links = page.GetFlattenedTree().OfType<Link>().ToList();
            Assert.That( links.Count, Is.EqualTo( 5 ) ); // max 5 pages
            XAssert.LinkEquals( new Link( PageName.Create( "p1" ) ), links[ 0 ] );
            XAssert.LinkEquals( new Link( PageName.Create( "p2" ) ), links[ 1 ] );
            XAssert.LinkEquals( new Link( PageName.Create( "p3" ) ), links[ 2 ] );
            XAssert.LinkEquals( new Link( PageName.Create( "p4" ) ), links[ 3 ] );
            XAssert.LinkEquals( new Link( PageName.Create( "p5" ) ), links[ 4 ] );
        }
    }
}
