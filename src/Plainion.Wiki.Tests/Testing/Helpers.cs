using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.UnitTests.Testing
{
    public static class Helpers
    {
        public static IEnumerable<IPageDescriptor> CreateUniqDummyPages()
        {
            return new IPageDescriptor[] 
                { 
                    CreateUniqEmptyDummyPage(),
                    CreateUniqEmptyDummyPage()
                };
        }

        public static IPageDescriptor CreateUniqEmptyDummyPage()
        {
            return new InMemoryPageDescriptor( CreateUniqPageName(), string.Empty );
        }

        public static IPageDescriptor CreateEmptyPage( string pageName )
        {
            return new InMemoryPageDescriptor( PageName.CreateFromPath( pageName ), string.Empty );
        }

        public static PageName CreateUniqPageName()
        {
            return PageName.Create( Guid.NewGuid().ToString() );
        }
    }
}
