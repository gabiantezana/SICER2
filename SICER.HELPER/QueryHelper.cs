using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;

namespace SICER.HELPER
{
    public class QueryHelper
    {
        private SapDbServerType SApDbServerType { get; }
        private string SqlQuery { get; set; }
        private string HanaQuery { get; set; }

        public QueryHelper(SapDbServerType dBServerType)
        {
            this.SApDbServerType = dBServerType;
            this.SqlQuery = string.Empty;
            this.HanaQuery = string.Empty;
        }

        public string QuerySapProveedor()
        {
            SqlQuery = "SELECT LicTradNum, CardName, validFor FROM [SBODemoCL].[dbo].OCRD WHERE CARDTYPE='S'";
            return ReturnQuery();
        }

        public string QuerySapMoneda()
        {
            SqlQuery = "SELECT DocCurrCod, CurrName, Locked from [SBODemoCL].[dbo].OCRN";
            return ReturnQuery();
        }

        public string QuerySapMetodosPago()
        {
            SqlQuery = "SELECT PayMethCod, Descript, Active from  [SBODemoCL].[dbo].OPYM";
            return ReturnQuery();
        }

        public string QuerySapCentroCostos()
        {
            SqlQuery = "SELECT PrcCode, PrcName, Active from  [SBODemoCL].[dbo].OPRC";
            return ReturnQuery();
        }

        public string QuerySapServicios()
        {
            SqlQuery = "SELECT ItemCode, ItemName, validFor from [SBODemoCL].[dbo].OITM";
            return ReturnQuery();
        }

        public string QuerySapAccounts()
        {
            SqlQuery = "SELECT AcctCode, AcctName, ValidFor  FROM [SBODemoCL].[dbo].OACT";
            return ReturnQuery();
        }

        private string ReturnQuery()
        {
            switch (SApDbServerType)
            {
                case SapDbServerType.Sql:
                    return SqlQuery;
                case SapDbServerType.Hana:
                    return HanaQuery;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
