
namespace Plainion.Wiki.Http
{
    public interface IServerSite
    {
        /// <summary>
        /// Location of the pages
        /// </summary>
        string DocumentRoot { get; }

        /// <summary>
        /// Location of client sciprts (e.g. JS, CSS) which are fetched dynamically from server.
        /// </summary>
        string ClientScriptsRoot { get; }

        int Port { get; }
    }
}
