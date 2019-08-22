using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_MVVM_Employees.View
{
    /// <summary>
    /// Заменяет значение пустой строки на строку из параметра конвертора
    /// </summary>
    class EmptyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string == string.Empty ? parameter : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string == string.Empty ? parameter : value;
        }
    }
}
