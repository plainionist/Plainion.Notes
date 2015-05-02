using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Plainion.Notebook.Model;
using Plainion.AppFw.Wpf;
using Plainion.AppFw.Wpf.Services;
using Plainion.AppFw.Wpf.ViewModels;
using Plainion.Prism.Interactivity;

namespace Plainion.Notebook
{
    public class Bootstrapper : BootstrapperBase<Shell>
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Prism automatically loads the module with that line
            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( GetType().Assembly ) );
            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( typeof( PopupWindowActionRegionAdapter ).Assembly ) );

            AggregateCatalog.Catalogs.Add( new TypeCatalog(
                typeof( PersistenceService<Project> ),
                typeof( ProjectLifecycleViewModel<Project> ),
                typeof( TitleViewModel<Project> )
                ) );
        }

        protected override CompositionContainer CreateContainer()
        {
            var container = base.CreateContainer();

            container.ComposeExportedValue( new PageViewModelFactory( container ) );

            return container;
        }
    }
}
