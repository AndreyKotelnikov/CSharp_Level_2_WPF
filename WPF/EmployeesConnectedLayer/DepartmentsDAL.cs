﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EmployeesConnectedLayer
{
    /// <summary>
    /// Класс для работы с данными из БД: DepartmentsDAL (сокращенно от 'Departments Data Access Layer" — "Уровень доступа к данным департаментов")
    /// </summary>
    public class DepartmentsDAL : IConnectedLayer
    {
        /// <summary>
        /// объект подключения к БД
        /// </summary>
        private SqlConnection connect = null;

        /// <summary>
        /// Скрипт SQL запроса на получение данных из БД
        /// </summary>
        private string sqlSelect = "SELECT  ID, Name FROM dbo.Departments;";

        /// <summary>
        /// Скрипт SQL запроса на добавление записи в БД и возвратом значения нового ID
        /// </summary>
        private string sqlInsert = "INSERT INTO dbo.Departments (Name) VALUES (@Name); SET @ID = @@IDENTITY;";

        /// <summary>
        /// Скрипт SQL запроса на удаление записи из БД по указанному ID
        /// </summary>
        private string sqlDelete = "DELETE FROM dbo.Departments WHERE ID = @ID;";

        /// <summary>
        /// Скрипт SQL запроса на изменение данных в записи в БД по указанному ID
        /// </summary>
        private string sqlUpdate = "UPDATE dbo.Departments SET Name = @Name WHERE ID = @ID;";


        /// <summary>
        /// Создаёт список параметров для запроса
        /// </summary>
        /// <returns>Список параметров для запроса</returns>
        private SqlParameter[] CreateParams()
        {
            SqlParameter[] paramArr =
            {
                new SqlParameter("@ID", SqlDbType.Int, 0, "ID"),
                new SqlParameter("@Name", SqlDbType.NVarChar, -1, "Name"),
            };
            return paramArr;
        }

        /// <summary>
        /// Создаёт и открывает подключение к БД
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public void OpenConnection(string connectionString = null)
        {
            try
            {
                if (connectionString != null) { ConnectionString.Text = connectionString; }
                connect = new SqlConnection(ConnectionString.Text);
                connect.Open();
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw (ex);
            }
        }

        /// <summary>
        /// Закрывает подключение к БД
        /// </summary>
        public void CloseConnection()
        {
            connect.Close();
        }

        /// <summary>
        /// Возвращает DataReader с результатами запроса SELECT
        /// </summary>
        /// <returns>DataReader с результатами запроса SELECT</returns>
        public SqlDataReader Select()
        {
            try
            {
                if (connect.State != ConnectionState.Open) { OpenConnection(); }
                SqlDataReader dataReader;

                using (SqlCommand cmd = new SqlCommand(sqlSelect, connect))
                {
                    dataReader = cmd.ExecuteReader();
                }
                return dataReader;
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw (ex);
            }
        }

        /// <summary>
        /// Создаёт в БД новую запись с указанными значениями полей
        /// </summary>
        /// <param name="Name">Имя департамента</param>
        /// <returns>ID добавленной записи в БД</returns>
        public int Insert(string Name)
        {
            int result;
            try
            {
                if (connect.State != ConnectionState.Open) { OpenConnection(); }
                using (SqlCommand cmd = new SqlCommand(sqlInsert, connect))
                {
                    SqlParameter[] arr = CreateParams();

                    arr[0].Direction = ParameterDirection.Output;

                    arr[1].Value = Name;

                    cmd.Parameters.AddRange(arr);

                    cmd.ExecuteNonQuery();

                    result = (int)cmd.Parameters["@ID"].Value;
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                connect.Close();
            }
            return result;
        }

        /// <summary>
        /// Удаляет в БД запись с указанным ID
        /// </summary>
        /// <param name="ID">ID записи, которую нужно удалить</param>
        public void Delete(int ID)
        {
            try
            {
                if (connect.State != ConnectionState.Open) { OpenConnection(); }
                using (SqlCommand cmd = new SqlCommand(sqlDelete, connect))
                {
                    SqlParameter[] arr = CreateParams();
                    arr[0].Value = ID;
                    arr[0].SourceVersion = DataRowVersion.Original;
                    cmd.Parameters.Add(arr[0]);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                connect.Close();
            }
        }

        /// <summary>
        /// Меняет значения полей в записи БД с указанным ID
        /// </summary>
        /// <param name="ID">ID записи, данные в которой нужно изменить</param>
        /// <param name="Name">Имя департамента</param>
        public void Update(int ID, string Name)
        {
            try
            {
                if (connect.State != ConnectionState.Open) { OpenConnection(); }
                using (SqlCommand cmd = new SqlCommand(sqlUpdate, connect))
                {
                    SqlParameter[] arr = CreateParams();

                    arr[0].Value = ID;
                    arr[0].SourceVersion = DataRowVersion.Original;
                    arr[1].Value = Name;

                    cmd.Parameters.AddRange(arr);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                connect.Close();
            }

        }

        /// <summary>
        /// Количество записей в таблице БД 
        /// </summary>
        public int CountRows
        {
            get
            {
                int count;
                try
                {
                    if (connect.State != ConnectionState.Open) { OpenConnection(); }
                    using (SqlCommand cmd = new SqlCommand("CountRowsDep", connect))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        count = (int)cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {

                    throw (ex);
                }
                finally
                {
                    connect.Close();
                }
                return count;
            }
        }

        /// <summary>
        /// Создаёт адаптер для работы с данными в БД
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        /// <returns>Адаптер для работы с данными в БД</returns>
        public SqlDataAdapter GetDataAdapter(string connectionString = null)
        {
            if (connectionString != null) { ConnectionString.Text = connectionString; }
            if (connect == null) { connect = new SqlConnection(ConnectionString.Text); }
            SqlDataAdapter adapter = new SqlDataAdapter();

            SqlCommand cmdSelect = new SqlCommand(sqlSelect, connect);
            adapter.SelectCommand = cmdSelect;

            SqlCommand cmdInsert = new SqlCommand(sqlInsert, connect);
            SqlParameter[] arr = CreateParams();
            arr[0].Direction = ParameterDirection.Output;
            cmdInsert.Parameters.AddRange(arr);
            adapter.InsertCommand = cmdInsert;

            SqlCommand cmdDelete = new SqlCommand(sqlDelete, connect);
            arr = CreateParams();
            arr[0].SourceVersion = DataRowVersion.Original;
            cmdDelete.Parameters.Add(arr[0]);
            adapter.DeleteCommand = cmdDelete;

            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, connect);
            arr = CreateParams();
            arr[0].SourceVersion = DataRowVersion.Original;
            cmdUpdate.Parameters.AddRange(arr);
            adapter.UpdateCommand = cmdUpdate;

            return adapter;
        }

        /// <summary>
        /// Очищает таблицу с данными в БД
        /// </summary>
        public void ClearTable()
        {
            try
            {
                if (connect.State != ConnectionState.Open) { OpenConnection(); }
                using (SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Departments;", connect))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                connect.Close();
            }
        }
    }
}
