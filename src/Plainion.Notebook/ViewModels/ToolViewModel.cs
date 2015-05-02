using System;
using Plainion.Notebook.ViewModels;

namespace Plainion.Notebook.ViewModels
{
    abstract class ToolViewModel : PaneViewModel
    {
        private bool myIsVisible;

        public ToolViewModel( string name )
        {
            Name = name;
            Title = name;

            myIsVisible = true;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsVisible
        {
            get { return myIsVisible; }
            set { SetProperty( ref myIsVisible, value ); }
        }
    }
}
