using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkDataSet
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeesDataMSSQLEntities db = new EmployeesDataMSSQLEntities();

            //db.Employees.Add(new Employees() { Name = "Марат", Surname = "Маратович", Age = 32, DepID = 24 });
            //db.SaveChanges();

            foreach (var item in db.Employees)
            {
                Console.WriteLine($"{item} : {db.Departments?.Where(d => d.ID == item.DepID)?.FirstOrDefault()?.ToString()}");
            }

            Console.ReadLine();
        }
    }
}
