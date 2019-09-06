using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebServiceAPIDataBase.Models;

namespace WebServiceAPIDataBase.Controllers
{
    /// <summary>
    /// Контроллер для подключения к WebServiceAPI
    /// </summary>
    public class EmpController : ApiController
    {
        /// <summary>
        /// База данных
        /// </summary>
        private EmployeesDataMSSQLEntities employeesData = new EmployeesDataMSSQLEntities();

        /// <summary>
        /// GET запрос. Предоставляет список сущностей сотрудников
        /// </summary>
        /// <returns>Список сущностей сотрудников</returns>
        [Route ("GetListEmp")]
        public IEnumerable<Employees> GetEmployees() => employeesData.Employees;

        /// <summary>
        /// GET запрос.Предоставляет сущность сотрудника по указанному ID
        /// </summary>
        /// <param name="ID">ID сущности сотрудника</param>
        /// <returns>Сущность сотрудника по указанному ID</returns>
        [Route("GetListEmp/{id}")]
        public Employees GetEmployeeByID(int ID) => employeesData.Employees.FirstOrDefault(e => e.ID == ID);


        /// <summary>
        /// POST запрос. Изменяет поля сущности сотрудника в БД или добавляет новую сущность сотрудника в БД 
        /// </summary>
        /// <param name="emp">Сущность сотрудника для изменения или добавления</param>
        /// <returns>Сообщение ответа HTTP со словарём старого и нового значения ID (если ID не менялся, то запись в словарь не добавляется)</returns>
        //Формат получаемых данных: {"ID":69,"Name":"Семён2","Surname":"Семёнов2   ","Age":32,"DepID":24}
        [Route("AddEmp")]
        public HttpResponseMessage AddEmp([FromBody] Employees emp) => AddEntity(emp);


        /// <summary>
        /// POST запрос. Изменяет поля сущностей сотрудников в БД на основании указанного списка сотрудников или добавляет новые сущности сотрудников в БД  
        /// </summary>
        /// <param name="empList">Список сотрудников для изменения данных по ним в БД</param>
        /// <returns>Сообщение ответа HTTP со словарём старых и новых значений ID по изменённым и добавленным сущностям (если ID не менялся, то запись в словарь не добавляется)</returns>
        [Route("AddListEmp")]
        public HttpResponseMessage AddListEmp([FromBody] List<Employees> empList) => AddList(empList);

        /// <summary>
        ///  DELETE запрос. Удаляет сущности сотрудников в БД по указанным ID из списка
        /// </summary>
        /// <param name="listID">Список ID, сущности с которым нужно удалить в БД</param>
        /// <returns>Сообщение ответа HTTP</returns>
        [Route("DeleteListEmp")]
        public HttpResponseMessage DeleteListEmp([FromBody] List<int> listID) => DeleteList<Employees>(listID);

        /// <summary>
        /// GET запрос. Предоставляет список сущностей департаментов
        /// </summary>
        /// <returns>Список сущностей департаментов</returns>
        [Route("GetListDep")]
        public IEnumerable<Departments> GetListDep() => employeesData.Departments;

        /// <summary>
        /// GET запрос.Предоставляет сущность департамента по указанному ID
        /// </summary>
        /// <param name="ID">ID сущности департамента</param>
        /// <returns>Сущность департамента по указанному ID</returns>
        [Route("GetListDep/{id}")]
        public Departments GetDepartmentsByID(int ID) => employeesData.Departments.FirstOrDefault(d => d.ID == ID);

        /// <summary>
        /// POST запрос. Изменяет поля сущности департамента в БД или добавляет новую сущность департамента в БД 
        /// </summary>
        /// <param name="dep">Сущность департамента для изменения или добавления</param>
        /// <returns>Сообщение ответа HTTP со словарём старого и нового значения ID (если ID не менялся, то запись в словарь не добавляется)</returns>
        [Route("AddDep")]
        public HttpResponseMessage AddDep([FromBody] Departments dep) => AddEntity(dep);

        /// <summary>
        /// POST запрос. Изменяет поля сущностей департаментов в БД на основании указанного списка департаментов или добавляет новые сущности департаментов в БД  
        /// </summary>
        /// <param name="depList">Список департаментов для изменения данных по ним в БД</param>
        /// <returns>Сообщение ответа HTTP со словарём старых и новых значений ID по изменённым и добавленным сущностям (если ID не менялся, то запись в словарь не добавляется)</returns>
        [Route("AddListDep")]
        public HttpResponseMessage AddListDep([FromBody] List<Departments> depList) => AddList(depList);

        /// <summary>
        ///  DELETE запрос. Удаляет сущности департаментов в БД по указанным ID из списка
        /// </summary>
        /// <param name="listID">Список ID, сущности с которым нужно удалить в БД</param>
        /// <returns>Сообщение ответа HTTP</returns>
        [Route("DeleteListDep")]
        public HttpResponseMessage DeleteListDep([FromBody] List<int> listID) => DeleteList<Departments>(listID);

        /// <summary>
        /// Изменяет поля сущности в БД или добавляет новую сущность в БД
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Сущность, которую нужно изменить или добавить в БД</param>
        /// <returns>Сообщение ответа HTTP со словарём старого и нового значения ID (если ID не менялся, то запись в словарь не добавляется)</returns>
        private HttpResponseMessage AddEntity<T>(T entity) where T : class, IDataRow
        {
            DbSet<T> table = GetTable<T>();

            table.Add(entity as T);

            if (employeesData.SaveChanges() > 0)
            {
                int newID = table.OrderByDescending(e => e.ID).FirstOrDefault().ID;

                return Request.CreateResponse<int>(HttpStatusCode.OK, newID);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// POST запрос. Изменяет поля сущностей в БД на основании указанного списка или добавляет новые сущности в БД
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="list">Список сущностей, которые нужно изменить или добавить в БД</param>
        /// <returns>Сообщение ответа HTTP со словарём старых и новых значений ID по изменённым и добавленным сущностям (если ID не менялся, то запись в словарь не добавляется)</returns>
        private HttpResponseMessage AddList<T>(List<T>  list) where T : class, IDataRow
        {
            Dictionary<int, int> changeID = new Dictionary<int, int>();

            DbSet<T> table = GetTable<T>();

            foreach (var item in list)
            {
                IDataRow entity = table.FirstOrDefault(e => e.ID == item.ID);

                if (entity == null)
                {
                    int oldID = item.ID;
                    table.Add(item);
                    if (employeesData.SaveChanges() > 0)
                    {
                        if (oldID != item.ID) { changeID.Add(oldID, item.ID); }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    entity.Name = item.Name;

                    if (item is Employees)
                    {
                        Employees entityEmp = entity as Employees;
                        Employees itemEmp = item as Employees;
                        entityEmp.Surname = itemEmp.Surname;
                        entityEmp.Age = itemEmp.Age;
                        entityEmp.DepID = itemEmp.DepID;
                    }
                    
                    if (employeesData.SaveChanges() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
            }
            return Request.CreateResponse<Dictionary<int, int>>(HttpStatusCode.OK, changeID);
        }

        /// <summary>
        /// DELETE запрос. Удаляет сущности в БД по указанным ID из списка
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="listID">Список ID, сущности с которыми нужно удалить из БД</param>
        /// <returns>Сообщение ответа HTTP</returns>
        private HttpResponseMessage DeleteList<T>(List<int> listID) where T : class, IDataRow
        {
            DbSet<T> table = GetTable<T>();

            foreach (var item in listID)
            {
                IDataRow entity = table.FirstOrDefault(e => e.ID == item);

                if (entity == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                table.Remove(entity as T);
            }
            if (employeesData.SaveChanges() == listID.Count)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Получает таблицу с сущностями указанного типа из базы данных
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <returns>Таблица сущностей из БД</returns>
        private DbSet<T> GetTable<T>() where T : class
        {
            PropertyInfo propertyInfo = typeof(EmployeesDataMSSQLEntities).GetRuntimeProperty(typeof(T).Name);

            return (DbSet<T>)propertyInfo.GetValue(employeesData);
        }

        

        
    }
}
