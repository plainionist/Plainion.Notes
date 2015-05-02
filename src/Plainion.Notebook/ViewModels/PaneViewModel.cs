using Microsoft.Practices.Prism.Mvvm;

namespace Plainion.Notebook.ViewModels
{
    class PaneViewModel : BindableBase
    {
        private string myTitle;
        private string myContentId;
        private bool myIsSelected;
        private bool myIsActive;

        public string Title
        {
            get { return myTitle; }
            set { SetProperty( ref myTitle, value ); }
        }

        public string ContentId
        {
            get { return myContentId; }
            set { SetProperty( ref myContentId, value ); }
        }

        public bool IsSelected
        {
            get { return myIsSelected; }
            set { SetProperty( ref myIsSelected, value ); }
        }

        public bool IsActive
        {
            get { return myIsActive; }
            set { SetProperty( ref myIsActive, value ); }
        }
    }
}
