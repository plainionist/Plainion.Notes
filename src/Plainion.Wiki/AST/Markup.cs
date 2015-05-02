using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Baseclass of all markups like a <see cref="Link"/> or a
    /// <see cref="PageAttribute"/>
    /// </summary>
    [Serializable]
    public abstract class Markup : PageLeaf
    {
    }
}
