using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WPF_MVVM_Employees_DataTable.Views
{
    /// <summary>
    /// Класс для переопределения метода ProvideValue в классе MarkupExtension
    /// </summary>
    public class EnumToItemsSource : MarkupExtension
    {
        private readonly Type _type;

        public EnumToItemsSource(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Предоставляет список значений перечисления из атрибутов Description
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>Список значений перечисления из атрибутов Description</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _type.GetMembers().SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>()).Select(x => x.Description).ToList();
        }
    }
}
