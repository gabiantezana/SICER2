using System.Collections.Generic;
using System.Linq;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Sync;

namespace SICER.LOGIC.Sync
{
    public static class SapCentroCostoLogic
    {
        public static ListSapCentroCostoViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapCentroCostoViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapCentroCosto>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapCentroCosto> GetList(DataContext dataContext, int? dimCode, string filter)
        {
            return new SapCentroCostoDataAccess(dataContext).GetList(dimCode, filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter, int? dimCode)
        {
            return GetList(dataContext, dimCode, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.SapCentroCostoOcrCode,
                text = x.OcrCode + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.OcrName,
            });
        }

        public static IPagedList<SapCentroCosto> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, null, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }
    }
}
