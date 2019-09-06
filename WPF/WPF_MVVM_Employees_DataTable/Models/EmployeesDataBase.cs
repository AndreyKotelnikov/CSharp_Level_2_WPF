using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeesConnectedLayer;
using WPF_MVVM_Employees_DataTable.ViewModels;

namespace WPF_MVVM_Employees_DataTable.Models
{
    /// <summary>
    /// Model для получения данных для таблиц ModelView из БД MSSQL
    /// </summary>
    class EmployeesDataBase : IEmployeesData, IDepartmentsData
    {
        /// <summary>
        /// Ссылка на единственный экземпляр объекта
        /// </summary>
        private static EmployeesDataBase instance;

        /// <summary>
        /// Ссылка на единственный экземпляр объекта
        /// </summary>
        public static IEmployeesData Instance
        {
            get { return instance ?? (instance = new EmployeesDataBase()); }
        }

        /// <summary>
        /// Защищённый конструктор класса
        /// </summary>
        protected EmployeesDataBase() { }

        /// <summary>
        /// Строка подключения
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Адаптер для таблицы сотрудников
        /// </summary>
        private SqlDataAdapter dataAdapterEmp;

        /// <summary>
        /// Таблица сотрудников
        /// </summary>
        private DataTable dataEmp;


        /// <summary>
        /// Адаптер для таблицы департаментов
        /// </summary>
        private SqlDataAdapter dataAdapterDep;

        /// <summary>
        /// Таблица департаментов
        /// </summary>
        private DataTable dataDep;


        /// <summary>
        /// Заполняет данные в таблице сотрудников
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        /// <returns>Представление таблицы</returns>
        DataView IEmployeesData.GetDataView(string connectionString) => GetDataView(ref dataEmp, ref dataAdapterEmp, typeof(EmployeesDAL), connectionString);
        
        /// <summary>
        /// Обновляет данные в таблице сотрудников
        /// </summary>
        /// <returns>Представление таблицы</returns>
        DataView IEmployeesData.Update() => Update(dataEmp, dataAdapterEmp);

        /// <summary>
        /// Заполняет данные в таблице департаментов
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        /// <returns>Представление таблицы</returns>
        DataView IDepartmentsData.GetDataView(string connectionString) => GetDataView(ref dataDep, ref dataAdapterDep, typeof(DepartmentsDAL), connectionString);

        /// <summary>
        /// Обновляет данные в таблице департаментов
        /// </summary>
        /// <returns>Представление таблицы</returns>
        DataView IDepartmentsData.Update() => Update(dataDep, dataAdapterDep);


        /// <summary>
        /// Обновляет данные в указанной таблице
        /// </summary>
        /// <param name="dt">Таблица с данными</param>
        /// <param name="adapter">Адаптер для соединения с базой данных</param>
        /// <returns>Представление таблицы</returns>
        private DataView Update(DataTable dt, SqlDataAdapter adapter)
        {
            if (dt?.GetChanges() == null) { return null; }
            try
            {
                adapter.Update(dt);
            }
            catch (SqlException)
            {
                throw;
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// Заполняет данные в указанной таблице
        /// </summary>
        /// <param name="dt">Таблица с данными</param>
        /// <param name="adapter">Адаптер для соединения с базой данных</param>
        /// <param name="layerType">Тип класса, из которого получаем адаптер для работы с БД</param>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        /// <returns>Представление таблицы</returns>
        private DataView GetDataView(ref DataTable dt, ref SqlDataAdapter adapter, Type layerType, string connectionString)
        {
            
            if (connectionString != null && string.IsNullOrWhiteSpace(connectionString) || (connectionString == null && this.connectionString == null))
            { throw new ArgumentException("Требуется указать строку подключения!"); }

            if (dt != null && connectionString == this.connectionString) { return dt.DefaultView; }

            dt = new DataTable();

            try
            {
                if (adapter == null || this.connectionString != connectionString)
                {
                    this.connectionString = connectionString;
                    IConnectedLayer layer = Activator.CreateInstance(layerType) as IConnectedLayer;
                    adapter = layer.GetDataAdapter(this.connectionString);
                }

                adapter.Fill(dt);
            }
            catch (Exception)
            {
                this.connectionString = null;
                throw;
            }
            
            return dt.DefaultView;
        }


    }
}
