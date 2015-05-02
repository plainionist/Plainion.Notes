using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.Testing
{
    public class FakePageDescriptor : InMemoryPageDescriptor
    {
        public FakePageDescriptor( params string[] content )
            : this( content.ToList() )
        {
        }

        public FakePageDescriptor( IEnumerable<string> content )
            : base( PageName.Create( "TestPage" ), content )
        {
        }
    }
}
