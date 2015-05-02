using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plainion.Wiki.AST;
using Plainion.Wiki.DataAccess;
using Plainion.Wiki.Auditing;
using System.IO;
using Plainion.Wiki.Query;
using Plainion.Wiki.Rendering;

namespace Plainion.Wiki
{
    /// <summary/>
    public interface IEngine
    {
        /// <summary/>
        RenderingPipeline RenderingPipeline { get; }

        /// <summary/>
        IErrorPageHandler ErrorPageHandler { get; set; }

        /// <summary>
        /// Generic config which can be provided by the site.
        /// Structure: under root every class can its own config block.
        /// Inside this block the structure is class dependent.
        /// </summary>
        SiteConfig Config { get; }

        /// <summary/>
        IAuditingLog AuditingLog { get; }

        /// <summary/>
        void Render(PageName pageName, Stream output);

        /// <summary/>
        void Render(IPageDescriptor pageDescriptor, Stream output);

        /// <summary/>
        void Render(PageBody pageBody, Stream output);

        /// <summary/>
        IPageDescriptor Find(PageName pageName);

        /// <summary/>
        PageBody Get(PageName pageName);

        /// <summary/>
        void Create(PageName pageName, IEnumerable<string> pageContent);

        /// <summary/>
        void Delete(PageName pageName);

        /// <summary/>
        void Update(PageName pageName, IEnumerable<string> pageContent);

        /// <summary/>
        QueryEngine Query { get; }

        ///// <summary/>
        void Move(PageName pageName, PageNamespace newNamespace);
    }
}
