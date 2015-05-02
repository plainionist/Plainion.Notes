using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Plainion.Wiki.Utils;

namespace Plainion.Wiki.Rendering
{
    /// <summary>
    /// Resolves and contains all configured rendering steps.
    /// </summary>
    [Export( typeof( RenderingStepCatalog ) )]
    public class RenderingStepCatalog : AbstractPluginCatalog<int, IRenderingStep, IRenderingStepMetadata>
    {
        /// <summary/>
        protected override int GetKey( IRenderingStepMetadata metadata )
        {
            return metadata.Stage;
        }
    }
}
