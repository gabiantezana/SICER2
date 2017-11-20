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
    public class OstcDataAccess
    {
        private DataContext DataContext { get; set; }

        public OstcDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OSTC, companyEntity);
                var localListDefault = DataContext.Context.OSTC.ToArray();
                var localList = localListDefault;

                var sapList = DataContext.Context.Database.SqlQuery<OSTC>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.Code)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.OSTC.Find(item.Code);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.Code == item.Code);
                        if (itemInSap != null)
                        {
                            itemInLocal = itemInSap;
                            DataContext.Context.Entry(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                        else
                        {
                            DataContext.Context.OSTC.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.OSTC.Add(item);
                        DataContext.Context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public IEnumerable<OSTC> GetList(string filter)
        {
            //TODO: FILTERS
            var query = DataContext.Context.OSTC.AsQueryable();
            return string.IsNullOrEmpty(filter)
                ? query
                : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x =>
                    x.Name.ToLower().Contains(token)));
        }
    }
}
