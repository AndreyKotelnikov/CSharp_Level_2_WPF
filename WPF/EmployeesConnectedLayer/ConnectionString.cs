using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesConnectedLayer
{
    /// <summary>
    /// Статический класс со строкой подключения к базе данных
    /// </summary>
    public static class ConnectionString
    {
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public static string Text { get; set; }
    }
}
