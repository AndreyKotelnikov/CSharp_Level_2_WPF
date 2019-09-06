using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{
    /// <summary>
    /// Статический класс для определения методов проверки структур таблиц для ViewModels
    /// </summary>
    static class DataBaseValidation
    {
        /// <summary>
        /// Проверяет указанную таблицу на соответсвие структуры данных для работы со списком сотрудников
        /// </summary>
        /// <param name="dataEmp">Таблица, которую требуется проверить на соответствие</param>
        public static void EmpValidete(DataView dataEmp)
        {
            var columns = dataEmp.Table.Columns;
            if (columns.Contains("ID") && columns.Contains("Name") && columns.Contains("Surname") && columns.Contains("DepID"))
            {
                return;
            }
            throw new Exception("Некорректные столбцы в таблице с сотрудниками");
        }

        /// <summary>
        /// Проверяет указанную таблицу на соответсвие структуры данных для работы со списком департаментов
        /// </summary>
        /// <param name="dataDep">Таблица, которую требуется проверить на соответствие</param>
        public static void DepValidete(DataView dataDep)
        {
            var columns = dataDep.Table.Columns;
            if (columns.Contains("ID") && columns.Contains("Name"))
            {
                return;
            }
            new Exception("Некорректные столбцы в таблице с департаментами");
        }
    }
}
