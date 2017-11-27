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
    public class OcrdDataAccess
    {
        private DataContext DataContext { get; set; }

        public OcrdDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<OCRD> GetList(string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OCRD_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OCRD>(queryString).AsQueryable();

            list = string.IsNullOrEmpty(filter)
                 ? list
                 : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                     x.CardCode.ToSafeString().ToLower().Contains(token)
                     || x.CardName.ToSafeString().ToLower().Contains(token)
                     || x.LictradNum.ToSafeString().ToLower().Contains(token)));

            return list;
        }

        public OCRD GetItem(string cardCode)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OCRD_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OCRD>(queryString).AsQueryable();
            var item = list.FirstOrDefault(x => x.CardCode == cardCode);
            return item;
        }
    }
}
