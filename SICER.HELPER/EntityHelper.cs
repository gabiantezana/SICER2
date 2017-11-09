using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    #region SAP ENTITY TABLES

    public class OCRD 
    {
        [Key]
        public string CardCode { get; set; }
        public String LictradNum { get; set; }
        public String CardName { get; set; }
        public String validFor { get; set; }
        public string CardType { get; set; }

    }

    public class OITM
    {
        public String ItemCode { get; set; }
        public String ItemName { get; set; }
        public String validFor { get; set; }
    }

    public class OPRC
    {
        public String PrcCode { get; set; }
        public String PrcName { get; set; }
        public String Active { get; set; }
    }

    public class OPYM
    {
        public String PayMethCod { get; set; }
        public String Descript { get; set; }
        public String Active { get; set; }
    }

    public class OCRN
    {
        public String DocCurrCod { get; set; }
        public String CurrName { get; set; }
        public String Locked { get; set; }
    }

    public class OACT
    {
        public String AcctCode { get; set; }
        public String AcctName { get; set; }
        public String ValidFor { get; set; }
    }

    #endregion

}
