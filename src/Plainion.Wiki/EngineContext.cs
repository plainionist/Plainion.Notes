using System;
using Plainion.Wiki.AST;
using Plainion.Wiki.Auditing;
using Plainion.Wiki.Query;

namespace Plainion.Wiki
{
    /// <summary/>
    public class EngineContext
    {
        /// <summary/>
        public QueryEngine Query
        {
            get;
            set;
        }

        /// <summary/>
        public IAuditingLog AuditingLog
        {
            get;
            set;
        }

        /// <summary/>
        public Func<PageName, bool> PageExists
        {
            get;
            set;
        }

        /// <summary/>
        public Func<PageName, PageBody> GetPage
        {
            get;
            set;
        }

        /// <summary/>
        public Func<PageNamespace, string, PageName> FindPageByName
        {
            get;
            set;
        }

        /// <summary/>
        public SiteConfig Config
        {
            get;
            set;
        }
    }
}
