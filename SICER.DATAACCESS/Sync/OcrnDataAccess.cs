using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.DATAACCESS.Sync
{
    public class OcrnDataAccess
    {
        private DataContext DataContext { get; set; }

        public OcrnDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<OCRN> GetList(string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OCRN_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OCRN>(queryString).AsQueryable();

            list = string.IsNullOrEmpty(filter)
                ? list
                : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                    x.DocCurrCod.ToLower().Contains(token)
                    || x.CurrName.ToLower().Contains(token)));
            return list;
        }

        public OCRN GetItem(string DocCurrCod)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OCRN_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OCRN>(queryString).AsQueryable();
            var item = list.FirstOrDefault(x => x.DocCurrCod == DocCurrCod);
            return item;
        }
    }
}
