using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SICER.MODEL;

namespace SICER.HELPER
{
    public class QueryHelper
    {
        private BoDataServerTypes SApDbServerType { get; }
        private string SqlQuery { get; set; }
        private string HanaQuery { get; set; }

        public QueryHelper(BoDataServerTypes dBServerType)
        {
            this.SApDbServerType = dBServerType;
            this.SqlQuery = string.Empty;
            this.HanaQuery = string.Empty;
        }


        public string GetSyncQuery(SyncEntity syncEntity) //TODO: CHANGE FOR SAP OR SQL QUERY SYNTAX
        {
            var resourceFullName = Assembly.GetCallingAssembly().GetManifestResourceNames().ToList().FirstOrDefault(x => x.Contains(syncEntity.ToString()));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found for: " + syncEntity.ToString());

            using (var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceFullName))
            {
                if (stream == null) throw new Exception("A problem was encountered while system has reading the file.");
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            
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
