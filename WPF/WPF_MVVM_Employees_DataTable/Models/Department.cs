using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.Models
{
    /// <summary>
    /// Департамент
    /// </summary>
    class Department
    {
        /// <summary>
        /// Счётчик для отслеживание уникальности поля ID
        /// </summary>
        private static int countID = 0;

        /// <summary>
        /// Уникальный ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Department()
        {
            ID = countID;
            countID++;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="name">Наименование департамента</param>
        public Department(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Генерирует список департаментов
        /// </summary>
        /// <returns>Список департаментов</returns>
        public static IList<Department> GetItemsList()
        {
            List<Department> result = new List<Department>();

            result.Add(new Department("First"));
            result.Add(new Department("Second"));
            result.Add(new Department("Third"));
            result.Add(new Department("Fourth"));
            result.Add(new Department("Fifth"));

            return result;
        }
    }
}
