using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.HELPER;

namespace SICER.TESTER
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GetCompanyEntityFromFile("D:\\test1.xml", "SBO_JHOMERON");
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                Console.Read();
                throw;
            }
        }

        public static CompanyEntity GetCompanyEntityFromFile(string xmlParametersPath, string companyName)
        {
            var xml = System.IO.File.ReadAllText(xmlParametersPath);
            var list = SerializeHelper.XMLToObject(xml, typeof(ConnectionParameters));
            var item = ((ConnectionParameters) list).Companies.ToList().FirstOrDefault(x => x.CompanyDB.Equals(companyName));
            return item;
        }










    }
}
