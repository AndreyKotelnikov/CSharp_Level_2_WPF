using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebAppWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceWCF" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы ServiceWCF.svc или ServiceWCF.svc.cs в обозревателе решений и начните отладку.
    public class ServiceWCF : IServiceWCF
    {
        
        public void DoWork()
        {
            
        }

        public string Sum(int a, int b)
        {
            return $"{a} + {b} = {a + b}";
        }
    }
}
