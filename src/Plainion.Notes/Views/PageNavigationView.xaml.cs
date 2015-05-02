using System.ComponentModel.Composition;
using System.Windows.Controls;
using Plainion.Notes.ViewModels;

namespace Plainion.Notes.Views
{
    [Export( typeof( PageNavigationView ) )]
    public partial class PageNavigationView : UserControl
    {
        [ImportingConstructor]
        public PageNavigationView(PageNavigationViewModel model)
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
