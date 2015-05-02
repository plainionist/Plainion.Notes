using System;
using System.ComponentModel.Composition;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    [MetadataAttribute]
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class RenderingStepAttribute : ExportAttribute, IRenderingStepMetadata
    {
        /// <summary/>
        public RenderingStepAttribute( RenderingStage stage )
            : this( (int)stage )
        {
        }

        /// <summary/>
        public RenderingStepAttribute( int stage )
            : base( typeof( IRenderingStep ) )
        {
            Stage = stage;
        }

        /// <summary/>
        public int Stage
        {
            get;
            private set;
        }
    }
}
