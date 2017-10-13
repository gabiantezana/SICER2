using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.TESTER
{
    class Program
    {
        static void Main(string[] args)
        {
            DataContext dataContext = new DataContext();
            dataContext.Context = new SICEREntities();
            dataContext.SAPDbServerType = SAPDbServerType.SQL;

            var sadf = dataContext.Context.SP_SAPProveedor().ToList();
            Console.ReadLine();
        }
    }
}
