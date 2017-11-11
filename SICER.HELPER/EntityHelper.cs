using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace SICER.HELPER
{
    public class EntityHelper
    {

    }

    public class JsonEntity
    {
        public Int32 id { get; set; }
        public String text { get; set; }
    }

    public class JsonEntityTwoString
    {
        public String id { get; set; }
        public String text { get; set; }
    }

    public class TempDataEntity
    {
        public MessageType TipoMensaje { get; set; }
        public String Mensaje { get; set; }
    }

    public class CompanyEntity
    {
        public bool? XMLAsString { get; set; }
        public string Server { get; set; }
        public string LicenseServer { get; set; }
        public BoDataServerTypes DbServerType { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public BoSuppLangs language { get; set; }
        public bool UseTrusted { get; set; }
        public bool Connected { get; set; }

        public CompanyEntity()
        {
            XMLAsString = null;
            LicenseServer = String.Empty;
            DbUserName = String.Empty;
            DbPassword = String.Empty;
            CompanyDB = String.Empty;
            UserName = String.Empty;
            Password = String.Empty;
        }
    }
}
