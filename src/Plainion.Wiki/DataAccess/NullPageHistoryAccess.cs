using System.ComponentModel.Composition;

namespace Plainion.Wiki.DataAccess
{
    [Export( typeof( IPageHistoryAccess ) )]
    public class NullPageHistoryAccess : IPageHistoryAccess
    {
        public void CreateNewVersion( IPageDescriptor pageDescriptor )
        {
        }
    }
}
