using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_Employees_DataTable.ViewModels;

namespace WPF_MVVM_Employees_DataTable.Models
{
    /// <summary>
    /// Model для получения статических данных (без подключения к базе данных) для таблиц ModelView
    /// </summary>
    class EmployeesStaticData : IEmployeesData, IDepartmentsData
    {
        /// <summary>
        /// Ссылка на единственный экземпляр объекта
        /// </summary>
        private static EmployeesStaticData instance;

        /// <summary>
        /// Ссылка на единственный экземпляр объекта
        /// </summary>
        public static EmployeesStaticData Instance
        {
            get { return instance ?? (instance = new EmployeesStaticData()); }
        }

        /// <summary>
        /// Защищённый конструктор класса
        /// </summary>
        protected EmployeesStaticData() { }

        /// <summary>
        /// Таблица сотрудников
        /// </summary>
        private DataTable dtEmp;

        /// <summary>
        /// Таблица департаментов
        /// </summary>
        private DataTable dtDep;

        /// <summary>
        /// Заполняет данные в таблице сотрудников
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        /// <returns>Представление таблицы</returns>
        DataView IEmployeesData.GetDataView(string connectionString) => GetDataView<Employee>(ref dtEmp);

        /// <summary>
        /// Обновляет данные в таблице сотрудников
        /// </summary>
        /// <returns>Представление таблицы</returns>
        DataView IEmployeesData.Update() => Update(dtEmp);

        /// <summary>
        /// Заполняет данные в таблице департаментов
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        /// <returns>Представление таблицы</returns>
        DataView IDepartmentsData.GetDataView(string connectionString) => GetDataView<Department>(ref dtDep);

        /// <summary>
        /// Обновляет данные в таблице департаментов
        /// </summary>
        /// <returns>Представление таблицы</returns>
        DataView IDepartmentsData.Update() => Update(dtDep);

        /// <summary>
        /// Заполняет данные в указанной таблице
        /// </summary>
        /// <typeparam name="T">Тип сущности в таблице</typeparam>
        /// <param name="dt">Таблица для заполнения данными</param>
        /// <returns>Представление таблицы</returns>
        private DataView GetDataView<T>(ref DataTable dt)
        {
            if (dt == null)
            {
                IList<T> list = typeof(T).GetMethod("GetItemsList").Invoke(typeof(T), null) as IList<T>;
                dt = CreateDataTabale(list);
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// Создаёт и заполняет таблицу из элементов в указанном списке
        /// </summary>
        /// <typeparam name="T">Тип сущности в таблице</typeparam>
        /// <param name="list">Список сущностей, которыми нужно заполнить таблицу</param>
        /// <returns>Таблица с данными</returns>
        private DataTable CreateDataTabale<T>(IList<T> list)
        {
            string jsonStr = JsonConvert.SerializeObject(list);
            DataTable dt;
            dt = JsonExtensions.PreferInt32DeserializeObject<DataTable>(jsonStr);
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// Применяет изменения в указанной таблице
        /// </summary>
        /// <param name="dt">Таблица, в которой нужно принять изменения</param>
        /// <returns>Представдение таблицы</returns>
        private DataView Update(DataTable dt)
        {
            if (dt?.GetChanges() != null) { dt.AcceptChanges(); }
            return null;
        }

        
    }
}
