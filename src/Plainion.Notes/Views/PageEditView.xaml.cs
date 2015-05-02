using System.ComponentModel.Composition;
using System.Windows.Controls;
using Plainion.Notes.ViewModels;

namespace Plainion.Notes.Views
{
    [Export( CompositionNames.PageEditView )]
    public partial class PageEditView : UserControl
    {
        [ImportingConstructor]
        public PageEditView( PageEditViewModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
