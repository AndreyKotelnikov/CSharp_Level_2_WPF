using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{
    /// <summary>
    /// Интерфейст для работы с данными в таблице департаментов
    /// </summary>
    interface IDepartmentsData
    {
        /// <summary>
        /// Формирует представление таблицы департаментов
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <returns>Представление таблицы департаментов</returns>
        DataView GetDataView(string connectionString = null);

        /// <summary>
        /// Обновляет данные в источнике данных на основании изменений в таблице департаментов
        /// </summary>
        /// <returns>Представление таблицы департаментов</returns>
        DataView Update();
    }
}
