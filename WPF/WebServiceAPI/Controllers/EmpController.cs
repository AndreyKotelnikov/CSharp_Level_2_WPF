using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServiceAPI.Models;

namespace WebServiceAPI.Controllers
{
    public class EmpController : ApiController
    {
        private static IList<Employee> empList;


        public IEnumerable<Employee> GetAllEmployees()
        {
            empList = Employee.GetEmployees();
            return empList;
        }

        public IHttpActionResult GetEmployee(int ID)
        {
            Employee emp = empList.FirstOrDefault(e => e.ID == ID);
            if (emp == null) { return NotFound(); }
            return Ok(emp);
        }
    }
}
