using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SAPbobsCOM;
using SICER.MODEL;

namespace SICER.HELPER
{
    public class QueryHelper
    {
        private BoDataServerTypes SApDbServerType { get; }
        private string SqlQuery { get; set; }
        private string HanaQuery { get; set; }

        public QueryHelper()
        {
            //this.SApDbServerType = dBServerType;
            //this.SqlQuery = string.Empty;
            //this.HanaQuery = string.Empty;
        }


        public string GetQueryString(QueryFileName queryFileName, string companyName) //TODO: CHANGE FOR SAP OR SQL QUERY SYNTAX
        {
            if (string.IsNullOrEmpty(companyName)) throw new Exception("User has not any company associated.");
            var path = HttpRuntime.AppDomainAppPath + "\\Parameters\\ConnectionParameters.xml";

            var companyEntity = CompanyHelper.GetCompanyEntityFromFile(path, companyName);
            if (companyEntity == null)
                throw new Exception("No se encuentra el archivo de configuración para esta compañía.");

            string prefixName;
            switch (companyEntity.DbServerType)
            {
                case BoDataServerTypes.dst_MSSQL:
                case BoDataServerTypes.dst_MSSQL2005:
                case BoDataServerTypes.dst_MSSQL2008:
                case BoDataServerTypes.dst_MSSQL2012:
                case BoDataServerTypes.dst_MSSQL2014:
                    prefixName = "Sql.";
                    break;
                case BoDataServerTypes.dst_HANADB:
                    prefixName = "Hana.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //GET RESOURCE BY NAME
            var resourcesList = Assembly.GetCallingAssembly().GetManifestResourceNames().Where(x=> x.Contains(prefixName)).ToList();
            var resourceFullName = resourcesList.FirstOrDefault(x => x.Contains(queryFileName.ToString()));

            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found for: " + prefixName + queryFileName.ToString());

            //READ RESOURCE
            string query;
            using (var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceFullName))
            {
                if (stream == null) throw new Exception("A problem was encountered while system has reading the file.");
                using (var reader = new StreamReader(stream))
                {
                    query = reader.ReadToEnd();
                }
            }

            //REPLACE STRING RESOURCE
            query = query.Replace(ConstantHelper.SBO_TEST001, "[" + companyEntity.CompanyDB + "]");
            return query;
        }

        public string GetUserFieldId()
        {
            var resourceFullName = Assembly.GetCallingAssembly().GetManifestResourceNames().ToList().FirstOrDefault(x => x.Contains(nameof(GetUserFieldId)));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found for: " + nameof(GetUserFieldId).ToString());

            using (var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceFullName))
            {
                if (stream == null) throw new Exception("A problem was encountered while system has reading the file.");
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
