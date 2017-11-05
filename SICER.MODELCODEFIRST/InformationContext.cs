using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace SICER.MODELCODEFIRST
{
    public class InformationContext
    {
        public HttpSessionStateBase Session { get; set; }
        public SicerContext Context { get; set; }
        public string CurrentCulture { get; set; }
        public string SystemNameSpace { get; set; }
        public HttpBrowserCapabilitiesBase Browser { get; set; }
        public SapDbServerType SapDbServerType { get; set; }
    }

    public enum SapDbServerType
    {
        Sql,
        Hana
    }
}
