using System.ComponentModel.Composition;
using Plainion.IO;

namespace Plainion.Wiki
{
    public class DefaultHttpCompositionDescriptor
    {
        [ImportingConstructor]
        public DefaultHttpCompositionDescriptor( [Import( CompositionContractNames.FileSystemRoot )]IDirectory fileSystemRoot)
        {
            DocumentRoot = fileSystemRoot;
        }

        [Export( CompositionContractNames.DocumentRoot )]
        public IDirectory DocumentRoot
        {
            get;
            private set;
        }
    }
}
