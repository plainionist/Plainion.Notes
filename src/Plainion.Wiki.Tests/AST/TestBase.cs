using System.Linq;
using Plainion.Wiki.AST;
using NUnit.Framework;
using System;

namespace Plainion.Wiki.UnitTests.AST
{
    public abstract class TestBase
    {
        [Test]
        public abstract void Clone_WhenCalled_ShouldNotThrow();
    }
}
