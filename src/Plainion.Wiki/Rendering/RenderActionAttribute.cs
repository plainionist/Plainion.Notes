using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Rendering
{
    /// <summary/>
    [MetadataAttribute]
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class RenderActionAttribute : ExportAttribute
    {
        /// <summary/>
        public RenderActionAttribute( Type contractName, Type nodeType )
            : base( contractName )
        {
            NodeType = nodeType;
        }

        /// <summary/>
        public RenderActionAttribute( string contractName, Type nodeType )
            : base( contractName )
        {
            NodeType = nodeType;
        }

        /// <summary/>
        public Type NodeType
        {
            get;
            private set;
        }
    }
}
