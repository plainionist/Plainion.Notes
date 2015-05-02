using System;
using Plainion;

namespace Plainion.Notebook.ViewModels
{
    class DisplayUrlRequest
    {
        public DisplayUrlRequest( Uri targetUri )
        {
            Contract.RequiresNotNull( targetUri, "targetUri" );

            TargetUri = targetUri;
            IsSourceView = false;
        }

        public Uri TargetUri { get; private set; }

        public bool IsSourceView { get; set; }
    }
}
