using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees.Data
{
    class Department
    {
        private static int countID = 0;

        public int ID { get; private set; }

        public string Name { get; set; }

       
        public Department()
        {
            ID = countID;
            countID++;
        }

        public Department(string name) : this()
        {
            Name = name;
        }

        public static Department[] GetDepartments()
        {
            var result = new Department[]
            {
                new Department("First"),
                new Department("Second"),
                new Department("Third"),
            };
            return result;
        }
    }

}

