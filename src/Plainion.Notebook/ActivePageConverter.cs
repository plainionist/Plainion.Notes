using System;
using System.Globalization;
using System.Windows.Data;
using Plainion.Notebook.ViewModels;

namespace Plainion.Notebook
{
    class ActivePageConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( value is PageViewModel )
            {
                return value;
            }

            return System.Windows.Data.Binding.DoNothing;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( value is PageViewModel )
            {
                return value;
            }

            return System.Windows.Data.Binding.DoNothing;
        }
    }
}
