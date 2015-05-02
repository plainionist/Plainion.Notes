using System;
using System.Collections.Generic;
using System.IO;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Rendering;
using NUnit.Framework;

namespace Plainion.Wiki.Html.UnitTests.Rendering
{
    public class TestBase : IHtmlRenderActionContext
    {
        protected MemoryStream myOutputStream;
        protected RenderingContext myContext;

        [SetUp]
        public virtual void SetUpBase()
        {
            myOutputStream = new MemoryStream();

            myContext = new RenderingContext( myOutputStream );
            myContext.EngineContext = new EngineContext();
            myContext.EngineContext.Config = new SiteConfig();

            OnNestedRenderCall = null;
            Stylesheet = new HtmlStylesheet();
        }

        [TearDown]
        public void TearDown()
        {
            myContext.Dispose();
            myOutputStream.Dispose();
        }

        protected IEnumerable<string> GetRenderingOutput()
        {
            myContext.Dispose();
            myOutputStream.Dispose();
            
            return myOutputStream.ReadLines();
        }

        protected void Render( IRenderAction renderAction, PageLeaf node )
        {
            Root = node;

            renderAction.Render( node, this );
        }

        public RenderingContext RenderingContext
        {
            get { return myContext; }
        }

        public PageLeaf Root
        {
            get;
            private set;
        }

        public void Render( PageLeaf node )
        {
            if( OnNestedRenderCall != null )
            {
                OnNestedRenderCall( node );
            }
        }

        protected Action<PageLeaf> OnNestedRenderCall
        {
            get;
            set;
        }

        public HtmlStylesheet Stylesheet
        {
            get;
            set;
        }
    }
}
