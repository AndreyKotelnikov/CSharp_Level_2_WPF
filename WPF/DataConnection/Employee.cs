using System;
using System.Collections.Generic;
using System.Text;

namespace DataConnection
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    class Employee
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
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// ID департамента
        /// </summary>
        public int? DepID { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Employee()
        {
            ID = countID;
            countID++;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="age">Возраст</param>
        /// <param name="depID">ID департамента</param>
        public Employee(string name, string surname, int age, int? depID = null) : this()
        {
            Name = name;
            Surname = surname;
            Age = age;
            DepID = depID;
        }

        /// <summary>
        /// Генерирует список сотруднико
        /// </summary>
        /// <returns>Список сотрудников</returns>
        public static IList<Employee> GetEmployees()
        {
            List<Employee> result = new List<Employee>();

            result.Add(new Employee("Иван", "Иванов", 20, 23));
            result.Add(new Employee("Петр", "Петров", 22));
            result.Add(new Employee("Сидор", "Сидоров", 23));
            result.Add(new Employee("Андрей", "Андреев", 27, 23));
            result.Add(new Employee("Семён", "Семёнов", 32));
            result.Add(new Employee("Илья", "Ильин", 41));
            result.Add(new Employee("Антон", "Антонов", 78, 25));
            result.Add(new Employee("Алексей", "Алексеев", 57));
            result.Add(new Employee("Евгений", "Евгениев", 16));

            return result;
        }


    }
}
