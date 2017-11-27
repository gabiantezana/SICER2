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
    public class OicdDataAccess
    {
        private DataContext DataContext { get; set; }

        public OicdDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<OICD> GetList(string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OICD_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OICD>(queryString).AsQueryable();

            list = string.IsNullOrEmpty(filter)
                ? list
                : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                    x.Code.ToLower().Contains(token)
                    || x.Name.ToLower().Contains(token)));
            return list;
        }
    }
}
