using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EmployeesConnectedLayer
{
    /// <summary>
    /// Интерфейс для получения адаптера для работы с данными из БД
    /// </summary>
    public interface IConnectedLayer
    {
        /// <summary>
        /// Создаёт адаптер для работы с данными в БД
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        /// <returns>Адаптер для работы с данными в БД</returns>
        SqlDataAdapter GetDataAdapter(string connectionString);
    }
}
