using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace WepClientAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = @"https://localhost:44307/GetListEmp";

            string urlAdd = @"https://localhost:44307/AddEmp";

            string urlAddList = @"https://localhost:44307/AddListEmp";

            string urlDeleteList = @"https://localhost:44307/DeleteListEmp";

            HttpClient client = new HttpClient();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonObj;

            #region AddListEmp
            //Employee emp = new Employee("Семён111223344455", "Семёнов111223344455", 32);
            //emp.ID = 90;

            //Employee emp2 = new Employee("Семён16", "Семёнов16", 32);


            //List<Employee> empListAdd = new List<Employee>();
            //empListAdd.Add(emp);
            //empListAdd.Add(emp2);

            //Print(empListAdd);
            //Console.WriteLine();


            //jsonObj = serializer.Serialize(empListAdd);


            //StringContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
            //var res = client.PostAsync(urlAddList, content).Result.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(res);

            //Dictionary<string, int> changeID = serializer.Deserialize<Dictionary<string, int>>(res);

            //foreach (var item in changeID)
            //{
            //    empListAdd.Find(e => e.ID == int.Parse(item.Key)).ID = item.Value;
            //}

            //Print(empListAdd);

            #endregion


            #region DeleteList


            //List<int> delID = new List<int>();
            //delID.Add(89);
            //delID.Add(94);

            //jsonObj = serializer.Serialize(delID);
            //Console.WriteLine(jsonObj);
            //Console.WriteLine();

            //StringContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            ////var res = client.PostAsync(urlDeleteList, content).Result;
            ////Console.WriteLine(res);
            ////Console.WriteLine();

            //HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("Delete"), urlDeleteList);
            //requestMessage.Content = content;
            //var res2 = client.SendAsync(requestMessage).Result;
            //Console.WriteLine(res2);
            //Console.WriteLine();

            #endregion

            client.DefaultRequestHeaders.Add("Accept", "Application/json");
            var res1 = client.GetStringAsync(url).Result;
            Console.WriteLine(res1);
            Console.WriteLine();

            //List<Employee> empList = serializer.Deserialize<List<Employee>>(res1);

            //DataTable empList = serializer.Deserialize<DataTable>(res1);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(res1);

            foreach (DataRow item in dt.Rows)
            {
                Console.WriteLine($"ID: {item[0]} Name: {item[1]}  Surname {item[2]} ");
            }

            //Print(empList);

            Console.WriteLine();

            

            //WebClient client = new WebClient() { Encoding = Encoding.UTF8};


            //Console.WriteLine(client.DownloadString(url));
            //Console.WriteLine();
            //Console.WriteLine(client.DownloadString(url + "/31"));
            Console.ReadLine();
        }

        static void Print(List<Employee> empList)
        {
            foreach (var item in empList)
            {
                Console.WriteLine($"{item.ID} {item.Name}");
            }
        }
    }
}
