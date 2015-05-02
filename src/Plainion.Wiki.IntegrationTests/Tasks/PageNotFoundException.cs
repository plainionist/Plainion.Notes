using System;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.IntegrationTests.Tasks
{

    public class PageNotFoundException : ApplicationException
    {
        public PageNotFoundException( PageName missingPage )
            : base( "No such page" )
        {
            MissingPage = missingPage;

            this.AddContext( "MissingPage", MissingPage );
        }

        public PageName MissingPage
        {
            get;
            private set;
        }
    }
}
