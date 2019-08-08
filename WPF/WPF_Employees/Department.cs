using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Employees
{
    public class Department
    {
        /// <summary>
        /// Используется для контроля уникальности экземпляра класа
        /// </summary>
        private static int countID = 0;
        /// <summary>
        /// Уникальный идентификатор экземпляра класса
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Название департамента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Конструктор класса с параметром
        /// </summary>
        /// <param name="name">Наименование департамента</param>
        public Department(string name)
        {
            ID = countID;
            countID++;
            Name = name;
        }
        /// <summary>
        /// Переопределение метода
        /// </summary>
        /// <returns>Возвращает представление экземпляра класса в виде строки</returns>
        public override string ToString()
        {
            return $"{Name} (id={ID})";
        }
    }
}
