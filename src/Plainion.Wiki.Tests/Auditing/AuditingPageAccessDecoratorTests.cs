using System;
using System.Collections.Generic;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.DataAccess;
using Moq;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Auditing
{
    [TestFixture]
    public class AuditingPageAccessDecoratorTests
    {
        private IPageAccess myPageAccess;
        private IAuditingLog myLog;

        [SetUp]
        public void SetUp()
        {
            myPageAccess = new Mock<IPageAccess> { DefaultValue = DefaultValue.Mock }.Object;
            myLog = new Mock<IAuditingLog> { DefaultValue = DefaultValue.Mock }.Object;
        }

        [Test]
        public void Ctor_WithNullPageAccess_Throws()
        {
            Assert.Throws<ArgumentNullException>( () => new AuditingPageAccessDecorator( null, myLog ) );
        }

        [Test]
        public void Ctor_WithNullAuditingLog_Throws()
        {
            Assert.Throws<ArgumentNullException>( () => new AuditingPageAccessDecorator( myPageAccess, null ) );
        }

        [Test]
        public void GetPages_WhenCalled_DelegatesToPageAccess()
        {
            Mock.Get( myPageAccess ).SetupGet( x => x.Pages ).Returns( new List<IPageDescriptor>() );

            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            var pages = decorator.Pages;

            Assert.That( pages, Is.SameAs( myPageAccess.Pages ) );
        }

        [Test]
        public void Find_WhenCalled_DelegatesToPageAccess()
        {
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );
            var pageName = PageName.Create( "a" );

            decorator.Find( pageName );

            Mock.Get( myPageAccess ).Verify( x => x.Find( It.IsAny<PageName>() ), Times.Once() );
        }

        [Test]
        public void Create_WhenCalled_DelegatesToPageAccess()
        {
            var descriptor = CreatePageDescriptor();
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            decorator.Create( descriptor );

            Mock.Get( myPageAccess ).Verify( x => x.Create( descriptor ), Times.Once() );
        }

        [Test]
        public void Create_WhenCalled_ActionWillBeLogged()
        {
            var descriptor = CreatePageDescriptor();
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            decorator.Create( descriptor );

            Mock.Get( myLog ).Verify( x => x.Log( It.Is<IAuditingAction>( action => action is CreateAction && action.RelatedPage == descriptor.Name ) ), Times.Once() );
        }

        [Test]
        public void Delete_WhenCalled_DelegatesToPageAccess()
        {
            var descriptor = CreatePageDescriptor();
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            decorator.Delete( descriptor );

            Mock.Get( myPageAccess ).Verify( x => x.Delete( descriptor ), Times.Once() );
        }

        [Test]
        public void Delete_WhenCalled_ActionWillBeLogged()
        {
            var descriptor = CreatePageDescriptor();
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            decorator.Delete( descriptor );

            Mock.Get( myLog ).Verify( x => x.Log( It.Is<IAuditingAction>( action => action is DeleteAction && action.RelatedPage == descriptor.Name ) ), Times.Once() );
        }

        [Test]
        public void Update_WhenCalled_DelegatesToPageAccess()
        {
            var descriptor = CreatePageDescriptor();
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            decorator.Update( descriptor );

            Mock.Get( myPageAccess ).Verify( x => x.Update( descriptor ), Times.Once() );
        }

        [Test]
        public void Update_WhenCalled_ActionWillBeLogged()
        {
            var descriptor = CreatePageDescriptor();
            var decorator = new AuditingPageAccessDecorator( myPageAccess, myLog );

            decorator.Update( descriptor );

            Mock.Get( myLog ).Verify( x => x.Log( It.Is<IAuditingAction>( action => action is UpdateAction && action.RelatedPage == descriptor.Name ) ), Times.Once() );
        }

        private IPageDescriptor CreatePageDescriptor()
        {
            var descriptor = new Mock<IPageDescriptor> { DefaultValue = DefaultValue.Mock };
            descriptor.SetupGet( x => x.Name ).Returns( PageName.Create( "a" ) );

            return descriptor.Object;
        }
    }
}
