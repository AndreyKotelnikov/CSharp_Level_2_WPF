using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{
    /// <summary>
    /// Тип источника данных, из которого заполняются таблицы с данными
    /// </summary>
    public enum TypeOfDataSource
    {
        [Description("Статические")]
        StaticData,
        [Description("База данны MSSQL")]
        SqlDataBase,
        [Description("WebServiceAPI")]
        WebServiceAPI
    }
}
