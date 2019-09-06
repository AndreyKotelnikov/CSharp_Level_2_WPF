using System;
using System.IO;
using System.Data.SqlClient;
using static System.Console;
using EmployeesConnectedLayer;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;

namespace DataConnection
{
    /// <summary>
    /// Класс для тестирования подключения к базе данных
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //string connectionString =
            //    @"  Data Source=(localdb)\MSSQLLocalDB;
            //        Initial Catalog=DataEmployees_22_08_2019;
            //        Integrated Security=True;
            //        Connect Timeout=30;
            //        Encrypt=False;
            //        TrustServerCertificate=False;
            //        ApplicationIntent=ReadWrite;
            //        MultiSubnetFailover=False";

            string connectionString = 
                @"  Data Source=andrey-kotelnik\sqlexpress;
                    Initial Catalog=EmployeesDataMSSQL;
                    Integrated Security=True";

            
            ConnectionString.Text = connectionString;
            EmployeesDAL employeesDAL = new EmployeesDAL();
            DepartmentsDAL departmentsDAL = new DepartmentsDAL();
            

            try
            {
                ReadDataFromBase(employeesDAL);
                employeesDAL.OpenConnection();
                //SqlDataAdapter dataAdapter = employeesDAL.GetDataAdapter();
                ////SqlParameter[] parameters = new SqlParameter[cmdTest.Parameters.Count];
                ////cmdTest.Parameters.CopyTo(parameters, 0);
                ////cmdTest.Parameters.Remove(parameters[0]);
                ////parameters[0].Value = 58;
                ////cmdTest.Parameters.Add(parameters[0]);

                ////cmdTest.ExecuteNonQuery();
                ////employeesDAL.CloseConnection();
                //DataTable table = new DataTable();
                //dataAdapter.Fill(table);
                //DataRow rowUpd = table.Rows[0];
                //var res = rowUpd.ItemArray.GetValue(1);
                //var f = rowUpd.ItemArray.IsReadOnly;
                //rowUpd.ItemArray[1] = "ИИИИИИИИ";
                //var arr = rowUpd.ItemArray;
                //arr[1] = "Иваныч2";
                //rowUpd.ItemArray = arr;
                //res = rowUpd.ItemArray.GetValue(1);

                //DataRow row = table.NewRow();
                //row[1] = "Andr2";
                //row[2] = "Andr2";
                //row[3] = 35;
                //table.Rows.Add(row);

                //table.AcceptChanges();
                //WriteLine($"The number of rows successfully updated: {dataAdapter.Update(table)}");


                //////employeesDAL.OpenConnection();
                var empList = Employee.GetEmployees();

                foreach (var item in empList)
                {
                    employeesDAL.Insert(item.Name, item.Surname, item.Age, item.DepID);
                }


                //WriteLine($"Номер вставленной строки: {employeesDAL.Insert("Й", "Й", 10, null)}");
                WriteLine($"Количество строк в таблице: {employeesDAL.CountRows}");
                //employeesDAL.Delete(25);
                //employeesDAL.Update(8, "8", "8", 8, 1);
                SqlDataReader reader;
                reader = employeesDAL.Select();
                WriteLine("Объект чтения данных --> " + reader.GetType().Name);

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Write($"|{reader[i],15}");
                    }
                    WriteLine();
                }

            }
            catch (SqlException ex)
            {

                WriteLine(ex.Message);
            }
            finally
            {
                employeesDAL.CloseConnection();
            }

            //try
            //{
            //    departmentsDAL.OpenConnection();
            //    WriteLine($"Номер вставленной строки: {departmentsDAL.Insert("qwerty")}");
            //    WriteLine($"Количество строк в таблице: {departmentsDAL.CountRows}");
            //    departmentsDAL.Delete(3);
            //    departmentsDAL.Update(1, "FFFFFF");
            //    SqlDataReader reader;
            //    reader = departmentsDAL.Select();
            //    WriteLine("Объект чтения данных --> " + reader.GetType().Name);

            //    while (reader.Read())
            //    {
            //        for (int i = 0; i < reader.FieldCount; i++)
            //        {
            //            Write($"|{reader[i],15}");
            //        }
            //        WriteLine();
            //    }

            //}
            //catch (SqlException ex)
            //{

            //    WriteLine(ex.Message);
            //}
            //finally
            //{
            //    departmentsDAL.CloseConnection();
            //}

            //SqlDataAdapter dataAdapterEmp = employeesDAL.GetDataAdapter();
            //DataTable dataEmp = new DataTable("Employees");
            

            //SqlDataAdapter dataAdapterDep = departmentsDAL.GetDataAdapter();
            //DataTable dataDep = new DataTable("Departments");

            //try
            //{
            //    dataAdapterEmp.Fill(dataEmp);
            //}
            //catch (SqlException ex)
            //{

            //    WriteLine(ex.Message);
            //}
            //try
            //{
            //    dataAdapterDep.Fill(dataDep);
            //}
            //catch (SqlException ex)
            //{

            //    WriteLine(ex.Message);
            //}

            //WriteLine($"Столбцы в Emp: { dataEmp.Columns.Count}");
            //WriteLine($"Столбцы в Dep: { dataDep.Columns.Count}");

            
            
        }

        private static void ReadDataFromBase(EmployeesDAL employeesDAL)
        {
            try
            {
                WriteLine($"Количество строк в таблице: {employeesDAL.CountRows}");
                SqlDataReader reader;
                reader = employeesDAL.Select();
                WriteLine("Объект чтения данных --> " + reader.GetType().Name);

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Write($"|{reader[i],15}");
                    }
                    WriteLine();
                }

            }
            catch (SqlException ex)
            {

                WriteLine(ex.Message);
            }
            finally
            {
                employeesDAL.CloseConnection();
            }
        }
    }
}
