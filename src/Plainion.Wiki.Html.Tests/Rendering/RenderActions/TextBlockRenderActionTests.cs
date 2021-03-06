﻿using System.Linq;
using Plainion.Wiki.AST;
using Plainion.Wiki.Html.Rendering.RenderActions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Plainion.Wiki.Html.UnitTests.Rendering.RenderActions
{
    [TestFixture]
    public class TextBlockRenderActionTests : TestBase
    {
        [Test]
        public void Render_WhenCalled_NotOutputWritten()
        {
            var para = new TextBlock();
            var renderAction = new TextBlockRenderAction();

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            Assert.That( output, Is.Empty );
        }

        [Test]
        public void Render_WithChildren_RenderingCalledForChildren()
        {
            var para = new TextBlock( "a" );
            var renderAction = new TextBlockRenderAction();
            OnNestedRenderCall = node => RenderingContext.Writer.WriteLine( "@@@" );

            Render( renderAction, para );

            var output = GetRenderingOutput().ToList();
            var expectedOutput = new[] { "@@@" };
            Assert.That( output, Is.EquivalentTo( expectedOutput ) );
        }
    }
}
