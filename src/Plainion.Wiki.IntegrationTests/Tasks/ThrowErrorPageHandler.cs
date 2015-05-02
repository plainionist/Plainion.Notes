using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki.IntegrationTests.Tasks
{
    /// <summary>
    /// Does not create error pages but throws exception instead.
    /// </summary>
    [Export( typeof( IErrorPageHandler ) )]
    public class ThrowErrorPageHandler : IErrorPageHandler
    {
        public IPageDescriptor CreatePageNotFoundPage( PageName missingPage )
        {
            throw new PageNotFoundException( missingPage );
        }

        public IPageDescriptor CreateGeneralErrorPage( string error )
        {
            throw new ApplicationException( error );
        }
    }
}
