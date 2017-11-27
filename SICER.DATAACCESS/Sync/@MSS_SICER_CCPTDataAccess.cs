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
    public class MSS_SICER_CCPTDataAccess
    {
        private DataContext DataContext { get; set; }
        public MSS_SICER_CCPTDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<MSS_SICER_CCPT> GetList(string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.MSS_SICER_CCPT_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<MSS_SICER_CCPT>(queryString).AsQueryable();

            list = string.IsNullOrEmpty(filter)
                ? list
                : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                    x.U_MSS_ACC.ToLower().Contains(token)
                    || x.U_MSS_DSC.ToLower().Contains(token)));
            return list;
        }

        public MSS_SICER_CCPT GetItem(string code)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.MSS_SICER_CCPT_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<MSS_SICER_CCPT>(queryString).AsQueryable();
            var item = list.FirstOrDefault(x => x.Code == code);
            return item;
        }
    }
}
