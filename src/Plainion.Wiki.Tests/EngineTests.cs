using System.IO;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Parser;
using Plainion.Wiki.Rendering;
using Plainion.Wiki.UnitTests.Testing;
using NUnit.Framework;
using Moq;

namespace Plainion.Wiki.UnitTests
{
    [TestFixture]
    public class EngineTests
    {
        private Mock<IPageAccess> myPageAccess;

        [SetUp]
        public void SetUp()
        {
            myPageAccess = new Mock<IPageAccess>() { DefaultValue = DefaultValue.Mock };
        }

        [Test]
        public void Render_NoSuchPage_ErrorPageHandlerCalled()
        {
            myPageAccess.Setup( x => x.Find( It.IsAny<PageName>() ) ).Returns<IPageDescriptor>( null );

            var engine = CreateEngine();

            var errorHandler = new Mock<IErrorPageHandler> { DefaultValue = DefaultValue.Mock };
            errorHandler.Setup( x => x.CreatePageNotFoundPage( It.IsAny<PageName>() ) ).Returns( new InMemoryPageDescriptor( PageName.Create( "PageNotFound" ) ) );
            
            engine.ErrorPageHandler = errorHandler.Object;

            engine.Render( PageName.Create( "a" ), new MemoryStream() );

            errorHandler.Verify( x => x.CreatePageNotFoundPage( It.IsAny<PageName>() ), Times.Once() );
        }
        
        [Test]
        public void Render_ExistingPage_RendererCalled()
        {
            myPageAccess.Setup( x => x.Find( It.IsAny<PageName>() ) ).Returns( new FakePageDescriptor() );
            var engine = CreateEngine();

            engine.Render( PageName.Create( "a" ), new MemoryStream() );

            Mock.Get( engine.RenderingPipeline.Renderer ).Verify( x => x.Render( It.IsAny<PageLeaf>(), It.IsAny<RenderingContext>() ), Times.Once() );
        }

        [Test]
        public void Find_WhenCalled_DelegatedToPageAccess()
        {
            var engine = CreateEngine();

            engine.Find( PageName.Create( "a" ) );

            myPageAccess.Verify( x => x.Find( It.IsAny<PageName>() ), Times.Once() );
        }

        [Test]
        public void Create_WhenCalled_DelegatedToPageAccess()
        {
            var engine = CreateEngine();

            engine.Create( PageName.Create( "a" ), new[] { string.Empty } );

            myPageAccess.Verify( x => x.Create( It.IsAny<IPageDescriptor>() ), Times.Once() );
        }

        [Test]
        public void Delete_WhenCalled_DelegatedToPageAccess()
        {
            myPageAccess.Setup( x => x.Find( It.IsAny<PageName>() ) ).Returns( new FakePageDescriptor() );
            var engine = CreateEngine();

            engine.Delete( PageName.Create( "a" ) );

            myPageAccess.Verify( x => x.Delete( It.IsAny<IPageDescriptor>() ), Times.Once() );
        }

        [Test]
        public void Update_WhenCalled_DelegatedToPageAccess()
        {
            var engine = CreateEngine();

            engine.Update( PageName.Create( "a" ), new[] { string.Empty } );

            myPageAccess.Verify( x => x.Update( It.IsAny<IPageDescriptor>() ), Times.Once() );
        }

        private IEngine CreateEngine()
        {
            var repository = new PageRepository( myPageAccess.Object, new ParserPipeline() );
            var engine = new Engine( repository );

            var catalog = new RenderingStepCatalog();
            var renderer = new Mock<IRenderer> { DefaultValue = DefaultValue.Mock }.Object;
            engine.RenderingPipeline = new RenderingPipeline( catalog, renderer );

            return engine;
        }
    }
}
