using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAPIDataBase.Controllers
{
    /// <summary>
    /// Интерфейс для работы с сущностями базы данных по свойствам ID и Name
    /// </summary>
    interface IDataRow
    {
        /// <summary>
        /// Свойство сущности ID 
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Свойство сущности Name
        /// </summary>
        string Name { get; set; }
    }
}
