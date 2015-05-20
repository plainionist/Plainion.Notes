using System;
using System.ComponentModel.Composition;
using Plainion.Wiki.Html.Rendering;
using Plainion.Wiki.Resources;
using Plainion.IO;

namespace Plainion.Wiki.Html
{
    public class DefaultHtmlCompsitionDescriptor
    {
        [ImportingConstructor]
        public DefaultHtmlCompsitionDescriptor( [Import( CompositionContractNames.FileSystemRoot )]IDirectory fileSystemRoot )
        {
            Func<string, string> ExistingFileNameOrNull = file => fileSystemRoot.File( file ).Exists ? fileSystemRoot.File( file ).Name : null;

            HtmlStylesheet = new HtmlStylesheet();
            HtmlStylesheet.ExternalStylesheet = ExistingFileNameOrNull( ResourceNames.CssStylesheet );
            HtmlStylesheet.ExternalJavascript = ExistingFileNameOrNull( ResourceNames.JavaScript );
        }

        [Export]
        public HtmlStylesheet HtmlStylesheet
        {
            get;
            private set;
        }
    }
}
