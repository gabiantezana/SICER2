using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.HELPER
{
    public class QueryHelper
    {
        private SAPDbServerType sAPDbServerType { get; set; }
        private String SQLQuery { get; set; }
        private String HanaQuery { get; set; }

        public QueryHelper(SAPDbServerType dBServerType)
        {
            this.sAPDbServerType = dBServerType;
            this.SQLQuery = String.Empty;
            this.HanaQuery = String.Empty;
        }

        public String QuerySapProveedor()
        {
            SQLQuery = "SELECT LicTradNum, CardName, validFor FROM [SBODemoCL].[dbo].OCRD WHERE CARDTYPE='S'";
            return ReturnQuery();
        }

        public String QuerySAPMoneda()
        {
            SQLQuery = "SELECT DocCurrCod, CurrName, Locked from [SBODemoCL].[dbo].OCRN";
            return ReturnQuery();
        }

        public String QuerySAPMetodosPago()
        {
            SQLQuery = "SELECT PayMethCod, Descript, Active from  [SBODemoCL].[dbo].OPYM";
            return ReturnQuery();
        }

        public String QuerySAPCentroCostos()
        {
            SQLQuery = "SELECT PrcCode, PrcName, Active from  [SBODemoCL].[dbo].OPRC";
            return ReturnQuery();
        }

        public String QuerySAPServicios()
        {
            SQLQuery = "SELECT ItemCode, ItemName, validFor from [SBODemoCL].[dbo].OITM";
            return ReturnQuery();
        }

        public String QuerySAPAccounts()
        {
            SQLQuery = "SELECT AcctCode, AcctName, ValidFor  FROM [SBODemoCL].[dbo].OACT";
            return ReturnQuery();
        }

        private String ReturnQuery()
        {
            switch (sAPDbServerType)
            {
                case SAPDbServerType.SQL:
                    return SQLQuery;
                case SAPDbServerType.HANA:
                    return HanaQuery;
                default:
                    throw new Exception("DBServerType not supported");
            }
        }
    }
}
