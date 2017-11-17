using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Sync;

namespace SICER.LOGIC.Sync
{
    public class SapTipoCambioLogic
    {
        public static ListSapTipoCambioViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapTipoCambioViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapTipoCambio>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapTipoCambio> GetList(DataContext dataContext, string filter)
        {
            return new SapTipoCambioDataAccess(dataContext).GetList(filter);
        }

        public static IPagedList<SapTipoCambio> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }
    }
}
