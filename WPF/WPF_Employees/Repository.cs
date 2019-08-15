using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Employees
{
    public class Repository
    {
        /// <summary>
        /// Список сотрудников
        /// </summary>
        public ObservableCollection<Employee> ListEmployees { get; set; }
        /// <summary>
        /// Список департаментов
        /// </summary>
        public ObservableCollection<Department> ListDepartments { get; set; } 
        /// <summary>
        /// Заполняет список департаментов начальными значениями
        /// </summary>
        public void FillDepartments()
        {
            ListDepartments = new ObservableCollection<Department>();
            ListDepartments.Add(new Department("Main"));
            ListDepartments.Add(new Department("Second"));
            ListDepartments.Add(new Department("Third"));
        }
        /// <summary>
        /// Заполняет список сотрудников начальными значениями
        /// </summary>
        public void FillEmployees()
        {
            ListEmployees = new ObservableCollection<Employee>();
            ListEmployees.Add(new Employee("Andrey", 23, ListDepartments[0]));
            ListEmployees.Add(new Employee("Olga", 27, ListDepartments[0]));
            ListEmployees.Add(new Employee("Alexey", 32, ListDepartments[1]));
            ListEmployees.Add(new Employee("Dmitriy", 30, null));
        }

    }
}
