using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.MODEL;
using SICER.HELPER;
using SICER.VIEWMODEL.Sync;

namespace SICER.LOGIC.Sync
{
    public class SapIndicatorLogic
    {
        public static ListSapIndicatorViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapIndicatorViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapIndicators>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapIndicators> GetList(DataContext dataContext, string filter)
        {
            return new SapIndicatorDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.Name
            });
        }

        public static IPagedList<SapIndicators> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToList().ToPagedList(page ?? 1, ConstantHelper.NUMEROFILASPORPAGINA);
        }

    }
}
