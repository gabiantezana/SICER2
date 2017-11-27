using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SAPbobsCOM;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.DATAACCESS.Sync
{
   public class OrttDataAccess
    {
        private DataContext DataContext { get; set; }

        public OrttDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<ORTT> GetList(string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.ORTT_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<ORTT>(queryString).AsQueryable();

            list = string.IsNullOrEmpty(filter)
                ? list
                : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                    x.Currency.ToLower().Contains(token)));
            return list;
        }

        public ORTT GetItem(string currency)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.ORTT_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<ORTT>(queryString).AsQueryable();
            var item = list.FirstOrDefault(x => x.Currency == currency);
            return item;
        }
    }
}
