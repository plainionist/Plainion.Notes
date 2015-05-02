using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;

namespace Plainion.Wiki.Query
{
    /// <summary/>
    public interface IQueryIterator
    {
        /// <summary/>
        PageLeaf CurrentNode { get; set; }

        /// <summary/>
        string GetIdentifierValue( string identfier );
    }
}
