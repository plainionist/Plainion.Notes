using System.ComponentModel.Composition;
using System.Windows;

namespace Plainion.Notes
{
    [Export]
    public partial class Shell : Window
    {
        [ImportingConstructor]
        public Shell( ShellViewModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
