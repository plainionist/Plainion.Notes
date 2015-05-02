using System.Windows;
using System.Windows.Controls;
using Plainion.Notebook.ViewModels;

namespace Plainion.Notebook.Views
{
    class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle
        {
            get;
            set;
        }

        public Style PageStyle
        {
            get;
            set;
        }

        public override Style SelectStyle( object item, DependencyObject container )
        {
            if ( item is ToolViewModel )
            {
                return ToolStyle;
            }

            if ( item is PageViewModel )
            {
                return PageStyle;
            }

            return base.SelectStyle( item, container );
        }
    }
}
