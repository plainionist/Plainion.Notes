
namespace Plainion.Wiki.Http
{
    public class DefaultServerSite : IServerSite
    {
        public DefaultServerSite( string documentRoot )
            : this( documentRoot, 8080 )
        {
        }

        public DefaultServerSite( string documentRoot, int port )
        {
            DocumentRoot = documentRoot;
            Port = port;
        }

        public string DocumentRoot
        {
            get;
            private set;
        }

        /// <summary>
        /// Server port.
        /// Default: 8080
        /// </summary>
        public int Port
        {
            get;
            set;
        }
    }
}
