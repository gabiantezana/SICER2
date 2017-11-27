using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.MODEL;
using SICER.HELPER;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.LOGIC.Sync
{
    public class SapIndicatorLogic
    {
        public static IEnumerable<OICD> GetList(DataContext dataContext, string filter)
        {
            return new OicdDataAccess(dataContext).GetList(filter);
        }

        public static IEnumerable<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.Name
            });
        }
    }
}
