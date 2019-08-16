using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees.Data
{
    class Employee
    {
        private static int countID = 0;

        public int ID { get; private set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public int? DepID { get; set; }

        public Employee()
        {
            ID = countID;
            countID++;
        }

        public Employee(string name, string surname, int age, int? depID = null) : this()
        {
            Name = name;
            Surname = surname;
            Age = age;
            DepID = depID;
        }

        public static Employee[] GetEmployees()
        {
            var result = new Employee[]
            {
                new Employee("Иван", "Иванов", 20, 1),
                new Employee("Петр", "Петров", 22),
                new Employee("Сидор", "Сидоров", 23)
            };
            return result;
        }
    }
}
