using System.ComponentModel.Composition;
using Plainion.Wiki.Resources;
using Plainion.IO;
using Plainion.Xaml;

namespace Plainion.Wiki
{
    public class DefaultFlatFileCompositionDescriptor
    {
        [ImportingConstructor]
        public DefaultFlatFileCompositionDescriptor( [Import( CompositionContractNames.FileSystemRoot )]IDirectory fileSystemRoot )
        {
            PageRoot = fileSystemRoot;
            HistoryRoot = fileSystemRoot;

            var configFile = PageRoot.File( ResourceNames.SiteConfigName );
            if( configFile.Exists )
            {
                var reader = new ValidatingXamlReader();
                SiteConfig = reader.Read<SiteConfig>( configFile.Path );
            }
            else
            {
                SiteConfig = new SiteConfig();
            }
        }

        [Export( CompositionContractNames.PageRoot )]
        public IDirectory PageRoot
        {
            get;
            private set;
        }

        [Export( CompositionContractNames.HistoryRoot )]
        public IDirectory HistoryRoot
        {
            get;
            private set;
        }

        [Export( CompositionContractNames.SiteConfig )]
        public SiteConfig SiteConfig
        {
            get;
            private set;
        }
    }
}
