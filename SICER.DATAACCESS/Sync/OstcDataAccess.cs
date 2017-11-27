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
    public class OstcDataAccess
    {
        private DataContext DataContext { get; set; }

        public OstcDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<OSTC> GetList(string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OSTC_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OSTC>(queryString).AsQueryable();

            list = string.IsNullOrEmpty(filter)
                ? list
                : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                    x.Name.ToLower().Contains(token)
                    || x.Code.ToLower().Contains(token)));
            return list;
        }

        public OSTC GetItem(string code)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OSTC_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OSTC>(queryString).AsQueryable();
            var item = list.FirstOrDefault(x => x.Code == code);
            return item;
        }

    }
}
