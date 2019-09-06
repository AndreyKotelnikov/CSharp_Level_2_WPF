using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
 


namespace WPF_MVVM_Employees_DataTable.Views
{
    /// <summary>
    /// Конвертер значений: конвертирует значение атрибута Description в значение самого элемента перечисления указанного типа
    /// </summary>
    public class EnumConverter : IValueConverter
    {
        /// <summary>
        /// Конвертирует элемент перечисления указанного типа в значение атрибута Description
        /// </summary>
        /// <param name="value">Элемент перечисления</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Тип перечисления</param>
        /// <param name="culture"></param>
        /// <returns>Значение атрибута Description</returns>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            foreach (var one in Enum.GetValues(parameter as Type))
            {
                if (value.Equals(one))
                {
                    string result = null;
                    var v1 = one.GetType();
                    var v2 = v1.GetMembers();
                    var v3 = v2.SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true));
                    var v4 = v3.ElementAt(((int)one));
                    result = (v4 as DescriptionAttribute).Description.ToString();
                    return result;
                }
            }
            return "";
        }

        /// <summary>
        /// Конвертирует значение атрибута Description в значение самого элемента перечисления указанного типа
        /// </summary>
        /// <param name="value">Значение атрибута Description</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Тип перечисления</param>
        /// <param name="culture"></param>
        /// <returns>Элемент перечисления</returns>
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            foreach (var one in Enum.GetValues(parameter as Type))
            {
                string result;
                var v1 = one.GetType();
                var v2 = v1.GetMembers();
                var v3 = v2.SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true));
                var v4 = v3.ElementAt(((int)one));
                result = (v4 as DescriptionAttribute).Description.ToString();

                if (value.ToString() == result)
                    return one;
            }
            return null;
        }
    }
}
