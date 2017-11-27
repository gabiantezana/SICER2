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
    public class OocrDataAccess
    {
        private DataContext DataContext { get; set; }

        public OocrDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<OOCR> GetList(int? dimCode, string filter)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OOCR_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OOCR>(queryString).AsQueryable();
            list = list.Where(x => x.DimCode == dimCode);

            list = string.IsNullOrEmpty(filter)
                ? list
                : filter.ToLower().Split(' ').Aggregate(list, (current, token) => current.Where(x =>
                    x.OcrCode.ToLower().Contains(token)
                    || x.OcrName.ToLower().Contains(token)
                    || x.DimCode.ToString().ToLower().Contains(token)));
            return list;
        }

        public OOCR GetItem(string ocrCode)
        {
            var queryString = new QueryHelper().GetQueryString(QueryFileName.OOCR_GetList, DataContext.Session.GetCompanyName());
            var list = DataContext.Context.Database.SqlQuery<OOCR>(queryString).AsQueryable();
            var item = list.FirstOrDefault(x => x.OcrCode == ocrCode);
            return item;
        }
    }
}
