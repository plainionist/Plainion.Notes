using System.Windows;
using System.Windows.Controls;
using Plainion.Notebook.ViewModels;
using Xceed.Wpf.AvalonDock.Layout;

namespace Plainion.Notebook.Views
{
    class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PageViewTemplate
        {
            get;
            set;
        }

        public DataTemplate NavigationViewTemplate
        {
            get;
            set;
        }

        public DataTemplate SearchResultsViewTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate( object item, DependencyObject container )
        {
            if ( item is PageViewModel )
            {
                return PageViewTemplate;
            }

            if( item is NavigationViewModel )
            {
                return NavigationViewTemplate;
            }

            if( item is SearchResultsViewModel )
            {
                return SearchResultsViewTemplate;
            }

            return base.SelectTemplate( item, container );
        }
    }
}
