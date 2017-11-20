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
    public class OstcLogic
    {
        public static ListOstcViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListOstcViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<OSTC>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<OSTC> GetList(DataContext dataContext, string filter)
        {
            return new OstcDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.Code + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.Name,
            });
        }

        public static IPagedList<OSTC> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }
    }
}
