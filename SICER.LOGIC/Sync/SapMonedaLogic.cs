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
    public class SapMonedaLogic
    {
        public static ListSapMonedaViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapMonedaViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapMoneda>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapMoneda> GetList(DataContext dataContext, string filter)
        {
            return new SapMonedaDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.SapMonedaDocCurrCod,
                text = x.SapMonedaDocCurrCod + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.CurrName
            });
        }

        public static IPagedList<SapMoneda> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

    }
}
