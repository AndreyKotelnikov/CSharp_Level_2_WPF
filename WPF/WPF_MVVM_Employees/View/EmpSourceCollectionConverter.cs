using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPF_MVVM_Employees.ViewModel;

namespace WPF_MVVM_Employees.View
{
    /// <summary>
    /// Цель конвертора - обнаружить событие, при котором коллекция элементов становится пустой.
    /// На изменения самой коллекции подписаться не получилось, поэтому ловим изменение свойства EmpItems.IsEmpty и проверяем количество элементов в коллекции.
    /// Саму коллекцию значений достаём через параметр конвертора
    /// </summary>
    public class EmpSourceCollectionConverter : IValueConverter
    {
        /// <summary>
        /// Измеряет количество элементов в коллекции
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Возвращает количество элементов в коллекции</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((parameter as EmployeesViewModel).EmpItems.SourceCollection as IList).Count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
