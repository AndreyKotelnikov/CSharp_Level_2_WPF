using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{
    /// <summary>
    /// Тип обновления данных
    /// </summary>
    public enum TypeOfUpdateData
    {
        [Description("Автоматический")]
        automatic,
        [Description("Ручной")]
        manual
    }
}
