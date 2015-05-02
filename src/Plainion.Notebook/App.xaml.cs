using System.Windows;
using System.Windows.Input;

namespace Plainion.Notebook
{
    public partial class App : Application
    {
        public App()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }

        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );

            new Bootstrapper().Run();
        }
    }
}
