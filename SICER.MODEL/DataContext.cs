using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SICER.MODEL
{
    public class DataContext
    {
        public HttpSessionStateBase Session { get; set; }
        public SICEREntities Context { get; set; }
        public String CurrentCulture { get; set; }
        public String SystemNameSpace { get; set; }
        public HttpBrowserCapabilitiesBase Browser { get; set; }
        public SapDbServerType SapDbServerType { get; set; }
    }

    public enum SapDbServerType
    {
        Sql,
        Hana,
    }
}
