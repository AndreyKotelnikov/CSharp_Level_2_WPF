using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Employees
{
    public class Employee
    {
        /// <summary>
        /// Используется для контроля уникальности экземпляра класа
        /// </summary>
        private static int countID = 0;
        /// <summary>
        /// Возраст сотрудника
        /// </summary>
        private int age;
        /// <summary>
        /// Уникальный идентификатор экземпляра класса
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Возраст сотрудника
        /// </summary>
        public int Age
        {
            get { return age; }
            set
            {
                if (value > 0 && value < 200)
                {
                    age = value;
                }
            }
        }
        /// <summary>
        /// Департамент, к которому принадлежит сотрудник
        /// </summary>
        public Department Department { get; set; }
        /// <summary>
        /// Конструктор класса без параметров
        /// </summary>
        public Employee()
        {
            ID = countID;
            countID++;
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="name">Имя сотрудника</param>
        /// <param name="age">Возраст сотрудника</param>
        /// <param name="department">Департамент, к которому принадлежит сотрудник</param>
        public Employee(string name, int age, Department department)
        {
            ID = countID;
            countID++;
            Name = name;
            Age = age;
            Department = department;
        }
    }
}
