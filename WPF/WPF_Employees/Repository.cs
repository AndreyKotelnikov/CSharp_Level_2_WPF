using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Employees
{
    class Repository
    {
        /// <summary>
        /// Список сотрудников
        /// </summary>
        public ObservableCollection<Employee> ListEmployees { get; set; } = new ObservableCollection<Employee>();
        /// <summary>
        /// Список департаментов
        /// </summary>
        public ObservableCollection<Department> ListDepartments { get; set; } = new ObservableCollection<Department>();
        /// <summary>
        /// Заполняет список департаментов начальными значениями
        /// </summary>
        public void FillDepartments()
        {
            ListDepartments.Add(new Department("Main"));
            ListDepartments.Add(new Department("Second"));
            ListDepartments.Add(new Department("Third"));
        }
        /// <summary>
        /// Заполняет список сотрудников начальными значениями
        /// </summary>
        public void FillEmployees()
        {
            ListEmployees.Add(new Employee("Andrey", 23, ListDepartments[0]));
            ListEmployees.Add(new Employee("Olga", 27, ListDepartments[0]));
            ListEmployees.Add(new Employee("Alexey", 32, ListDepartments[1]));
            ListEmployees.Add(new Employee("Dmitriy", 30, null));
        }

    }
}
