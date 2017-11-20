using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Sync;

namespace SICER.LOGIC.Sync
{
    public static class SapCuentaContableLogic
    {
        public static ListSapCuentaContableViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapCuentaContableViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapCuentaContable>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapCuentaContable> GetList(DataContext dataContext, string filter)
        {
            return new SapCuentaContableDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.U_MSS_ACC + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.U_MSS_DSC,
            });
        }

        public static IPagedList<SapCuentaContable> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }
    }
}
