//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebServiceAPIDataBase.Models
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using WebServiceAPIDataBase.Controllers;

    public partial class Employees : IDataRow
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> DepID { get; set; }

        [JsonIgnore] public virtual Departments Departments { get; set; }
    }
}