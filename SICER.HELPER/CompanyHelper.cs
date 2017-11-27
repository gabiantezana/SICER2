using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.HELPER
{
    public static class CompanyHelper
    {
        public static CompanyEntity GetCompanyEntityFromFile(string xmlParametersPath, string companyName)
        {
            var xml = System.IO.File.ReadAllText(xmlParametersPath);
            var list = SerializeHelper.XMLToObject(xml, typeof(ConnectionParameters));
            var item = ((ConnectionParameters)list).Companies.ToList().FirstOrDefault(x => x.CompanyDB.Equals(companyName));
            return item;
        }
    }
}

