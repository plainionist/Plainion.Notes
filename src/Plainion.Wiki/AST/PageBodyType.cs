using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plainion.Wiki.AST
{
    /// <summary>
    /// Defines the semantic content of the page body.
    /// </summary>
    [Serializable]
    public enum PageBodyType
    {
        /// <summary>
        /// The PageBody contains content from the page repository.
        /// </summary>
        Content,

        /// <summary>
        /// The PageBody contains a formular to create a new page.
        /// </summary>
        Create,

        /// <summary>
        /// The PageBody contains a formular to edit a page.
        /// </summary>
        Edit
    }
}
