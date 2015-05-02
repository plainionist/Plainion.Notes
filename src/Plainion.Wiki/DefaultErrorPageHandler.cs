using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki
{
    /// <summary>
    /// Default implementation of <see cref="IErrorPageHandler"/>
    /// </summary>
    [Export( typeof( IErrorPageHandler ) )]
    public class DefaultErrorPageHandler : IErrorPageHandler
    {
        public IPageDescriptor CreatePageNotFoundPage( PageName missingPage )
        {
            return new InMemoryPageDescriptor( PageName.Create( "PageNotFound" ),
                "No such page: " + missingPage.FullName );
        }

        public IPageDescriptor CreateGeneralErrorPage( string error )
        {
            return new InMemoryPageDescriptor( PageName.Create( "GeneralError" ),
                "!! General error occured",
                Environment.NewLine,
                "  ", error,
                Environment.NewLine );
        }
    }
}
