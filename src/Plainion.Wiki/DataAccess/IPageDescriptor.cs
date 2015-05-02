using Plainion.Wiki.AST;

namespace Plainion.Wiki.DataAccess
{
    /// <summary>
    /// Provides access to a single raw page in the system.
    /// </summary>
    public interface IPageDescriptor
    {
        /// <summary/>
        PageName Name { get; }

        /// <summary/>
        string[] GetContent();

        /// <summary/>
        bool Matches( string searchText );
    }
}
