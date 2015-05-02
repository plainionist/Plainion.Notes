
namespace Plainion.Wiki.Http
{
    public interface IServerSite
    {
        string DocumentRoot { get; }

        int Port { get; }
    }
}
