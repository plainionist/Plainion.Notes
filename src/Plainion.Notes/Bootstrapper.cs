using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Plainion.Notes.Model;
using Plainion.Notes.Services;
using Plainion.Notes.Views;
using Microsoft.Practices.Prism.Regions;
using Plainion.AppFw.Wpf;
using Plainion.AppFw.Wpf.Services;

namespace Plainion.Notes
{
    public class Bootstrapper : BootstrapperBase<Shell>
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Prism automatically loads the module with that line
            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( GetType().Assembly ) );

            AggregateCatalog.Catalogs.Add( new TypeCatalog(
                typeof( ProjectService<Project> )
                ) );
        }

        public override void Run( bool runWithDefaultConfiguration )
        {
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            base.Run( runWithDefaultConfiguration );

            Container.GetExportedValue<IRegionManager>().RegisterViewWithRegion( CompositionNames.NavigationRegion, typeof( PageNavigationView ) );

            Container.GetExportedValue<PageNavigationService>().NavigateToRead( Container.GetExportedValue<WikiService>().HomePage );
        }
    }
}
