using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;

namespace Plainion.Wiki.Parser
{
    /// <summary/>
    public interface IPageContentParser
    {
        /// <summary/>
        PageBody Parse();
    }
}
