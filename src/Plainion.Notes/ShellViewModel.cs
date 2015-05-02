using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;

namespace Plainion.Notes
{
    [Export]
    public class ShellViewModel : BindableBase
    {
        [ImportingConstructor]
        public ShellViewModel( IRegionManager regionManager )
        {
            CloseCommand = new DelegateCommand<Window>( w => w.Close() );
        }

        public ICommand CloseCommand { get; private set; }
    }
}
