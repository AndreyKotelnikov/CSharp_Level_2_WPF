using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WPF_MVVM_Employees_DataTable.ViewModels;

namespace WPF_MVVM_Employees_DataTable.Models
{
    class EmployeesWebAPI : IEmployeesData, IDepartmentsData
    {
        /// <summary>
        /// Таблица сотрудников
        /// </summary>
        private DataTable dtEmp;

        /// <summary>
        /// Таблица департаментов
        /// </summary>
        private DataTable dtDep;

        /// <summary>
        /// Строка подключения к ресурсу
        /// </summary>
        private string url;

        /// <summary>
        /// HTTP клиент
        /// </summary>
        private HttpClient client;

        //private JavaScriptSerializer serializer; - не получилось сериализовать в DataTable


        /// <summary>
        /// Ссылка на единственный экземпляр объекта
        /// </summary>
        private static EmployeesWebAPI instance;

        /// <summary>
        /// Ссылка на единственный экземпляр объекта
        /// </summary>
        public static EmployeesWebAPI Instance
        {
            get { return instance ?? (instance = new EmployeesWebAPI()); }
        }

        /// <summary>
        /// Защищённый конструктор класса
        /// </summary>
        protected EmployeesWebAPI()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "Application/json");
        }

        /// <summary>
        /// Заполняет данные в таблице сотрудников
        /// </summary>
        /// <param name="url">Строка подключения к ресурсу</param>
        /// <returns>Представление таблицы</returns>
        DataView IEmployeesData.GetDataView(string url) => GetDataView(ref dtEmp, url, "/GetListEmp");

        /// <summary>
        /// Заполняет данные в таблице департаментов
        /// </summary>
        /// <param name="url">Строка подключения к ресурсу</param>
        /// <returns>Представление таблицы</returns>
        DataView IDepartmentsData.GetDataView(string url) => GetDataView(ref dtDep, url, "/GetListDep");

        /// <summary>
        /// Обновляет данные в таблице сотрудников
        /// </summary>
        /// <returns>Представление таблицы</returns>
        DataView IEmployeesData.Update() => Update(ref dtEmp, "/DeleteListEmp", "/AddListEmp");

        /// <summary>
        /// Обновляет данные в таблице департаментов
        /// </summary>
        /// <returns>Представление таблицы</returns>
        DataView IDepartmentsData.Update() => Update(ref dtDep, "/DeleteListDep", "/AddListDep");

        /// <summary>
        /// Заполняет данные в указанной таблице
        /// </summary>
        /// <param name="dt">Таблица для заполнения данными</param>
        /// <param name="url">Строка подключения к ресурсу</param>
        /// <param name="urlMethod">Адрес страницы на ресурсе для выполнения GET запроса</param>
        /// <returns>Представление таблицы</returns>
        private DataView GetDataView(ref DataTable dt, string url, string urlMethod)
        {
            string result;
            try
            {
                if (url != null && string.IsNullOrWhiteSpace(url) || (url == null && this.url == null)) { throw new ArgumentException("Требуется указать строку подключения!"); }
                if (dt != null && url == this.url) { return dt.DefaultView; }
                this.url = url;
            
                result = client.GetStringAsync(this.url + urlMethod).Result;
            }
            catch 
            {
                throw;
            }
            //dt = JsonConvert.DeserializeObject<DataTable>(res, jsonSerializerSettings); //- создаёт колонки с типом long вместо int
            dt = JsonExtensions.PreferInt32DeserializeObject<DataTable>(result);
            dt.AcceptChanges();

            return dt.DefaultView;
        }

        /// <summary>
        /// Обновляет данные в указанной таблице
        /// </summary>
        /// <param name="dt">Таблица для обновления данных</param>
        /// <param name="urlMethodDelete">Адрес страницы на ресурсе для выполнения DELETE запроса</param>
        /// <param name="urlMethodUpdate">Адрес страницы на ресурсе для выполнения POST запроса</param>
        /// <returns>Представление таблицы</returns>
        private DataView Update(ref DataTable dt, string urlMethodDelete, string urlMethodUpdate)
        {
            DataTable dtChanges = dt?.GetChanges();
            if (dtChanges == null) { return null; }

            DeleteRows(dtChanges, urlMethodDelete);

            Dictionary<int, int> changeID = UpdateRows(dtChanges, urlMethodUpdate);

            ChangeID(dt, changeID);

            dt.AcceptChanges();

            return dt.DefaultView;
        }

        /// <summary>
        /// Удаляет на ресурсе все строки из указанной таблицы по ID
        /// </summary>
        /// <param name="dt">Таблица для получения списка ID для удаления записей на ресурсе</param>
        /// <param name="urlDelete">Адрес страницы на ресурсе для выполнения DELETE запроса</param>
        private void DeleteRows(DataTable dt, string urlDelete)
        {
            var rowsDelete = dt.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Deleted);
            if (rowsDelete.Count() > 0)
            {
                List<int> deleteID = new List<int>();
                foreach (var item in rowsDelete)
                {
                    int ID = (int)item["ID", DataRowVersion.Original];
                    deleteID.Add(ID);
                }

                string jsonObj = JsonConvert.SerializeObject(deleteID);
                StringContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("Delete"), url + urlDelete);
                requestMessage.Content = content;

                HttpResponseMessage httpResponse;
                try
                {
                    httpResponse = client.SendAsync(requestMessage).Result;
                }
                catch 
                {
                    throw;
                }

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) { throw new Exception("Ошибка при удалении данных через WebAPI"); }
            }
        }

        /// <summary>
        /// Обновляет на ресурсе записи или создаёт новые строки из указанной таблицы
        /// </summary>
        /// <param name="dt">таблица для обновления или создания новых записей на ресурсе</param>
        /// <param name="urlUpdate">Адрес страницы на ресурсе для выполнения POST запроса</param>
        /// <returns>Словарь значений ID, которые нужно обновить в таблице. Key = старое значение ID, Value = новое значение ID</returns>
        private Dictionary<int, int> UpdateRows(DataTable dt, string urlUpdate)
        {
            Dictionary<int, int> changeID = null;
            var rowsUpdate = dt.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted);
            if (rowsUpdate.Count() > 0)
            {
                DataTable tb = rowsUpdate.CopyToDataTable();
                string jsonObj = JsonConvert.SerializeObject(tb);

                StringContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse;
                try
                {
                    httpResponse = client.PostAsync(url + urlUpdate, content).Result;
                }
                catch
                {
                    throw;
                }
                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) { throw new Exception("Ошибка при обновлении данных через WebAPI"); }

                string responseContent = httpResponse.Content.ReadAsStringAsync().Result;

                changeID = JsonConvert.DeserializeObject<Dictionary<int, int>>(responseContent);
            }
            return changeID;
        }

        /// <summary>
        /// Обновляет значение ID в строках таблицы, указанных в словаре
        /// </summary>
        /// <param name="dt">Таблица, где требуется поменять значение ID в записях</param>
        /// <param name="changeID">Словарь значений ID, которые нужно обновить в таблице. Key = старое значение ID, Value = новое значение ID</param>
        private void ChangeID(DataTable dt, Dictionary<int, int> changeID)
        {
            if (changeID == null) { return; }
            foreach (var item in changeID)
            {
                DataRow row = dt.Rows.Find(item.Key);
                row.SetField<int?>("ID", item.Value);
            }
        }

    }
}
