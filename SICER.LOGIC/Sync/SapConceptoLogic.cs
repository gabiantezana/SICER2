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
    public static class SapConceptoLogic
    {
        public static ListSapConceptoViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapConceptoViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapConcepto>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapConcepto> GetList(DataContext dataContext, string filter)
        {
            return new SapConceptoDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext ,filter).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.U_MSS_ACCT + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.U_MSS_DSCRPT,
            });
        }

        public static IPagedList<SapConcepto> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }
    }
}
