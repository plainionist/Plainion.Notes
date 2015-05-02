using System;
using System.Collections.Generic;
using System.IO;
using Plainion.Testing;
using Plainion.Wiki.AST;
using Plainion.Wiki.Rendering;
using NUnit.Framework;

namespace Plainion.Wiki.UnitTests.Rendering
{
    public class TestBase : IRenderActionContext
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
    }
}
