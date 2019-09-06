﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{
    /// <summary>
    /// Интерфейст для работы с данными в таблице сотрудников
    /// </summary>
    public interface IEmployeesData
    {
        /// <summary>
        /// Формирует представление таблицы сотрудников
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <returns>Представление таблицы сотрудников</returns>
        DataView GetDataView(string connectionString = null);

        /// <summary>
        /// Обновляет данные в источнике данных на основании изменений в таблице сотрудников
        /// </summary>
        /// <returns>Представление таблицы сотрудников</returns>
        DataView Update();
    }
}
