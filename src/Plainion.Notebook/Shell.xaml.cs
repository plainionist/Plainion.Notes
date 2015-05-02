using System.ComponentModel.Composition;
using System.Windows;
using Xceed.Wpf.AvalonDock;

namespace Plainion.Notebook
{
    [Export( typeof( Shell ) )]
    public partial class Shell : Window
    {
        [ImportingConstructor]
        internal Shell( ShellViewModel model )
        {
            InitializeComponent();

            DataContext = model;
        }

        [Export]
        public DockingManager DockingManager
        {
            get { return myDockingManager; }
        }
    }
}
