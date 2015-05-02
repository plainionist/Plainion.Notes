using System.ComponentModel.Composition;
using System.Windows.Controls;
using Plainion.Notes.ViewModels;

namespace Plainion.Notes.Views
{
    [Export( CompositionNames.PageReadView )]
    public partial class PageReadView : UserControl
    {
        [ImportingConstructor]
        public PageReadView( PageReadViewModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
