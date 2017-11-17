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
    public class SapIndicatorDataAccess
    {
        private DataContext DataContext { get; set; }

        public SapIndicatorDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Sync()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.OICD);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OICD);
                var sapTableType = typeof(OICD);
                var localTableType = typeof(SapIndicators);

                var localListDefault = DataContext.Context.SapIndicators.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (OICD)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OICD>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.Code)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                var list = new List<Tuple<SyncType, dynamic>>();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = localListDefault.FirstOrDefault(x => x.Code == item.Code);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.Code == item.Code);
                        list.Add(itemInSap != null
                            ? new Tuple<SyncType, dynamic>(SyncType.Update, ConvertToEntity(SyncType.Update, itemInLocal, item))
                            : new Tuple<SyncType, dynamic>(SyncType.Delete, ConvertToEntity(SyncType.Delete, itemInLocal, item)));
                    }
                    else
                        list.Add(new Tuple<SyncType, dynamic>(SyncType.Create, ConvertToEntity(SyncType.Create, itemInLocal, item)));
                }

                new SyncDataAccess(DataContext).SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        private SapIndicators ConvertToEntity(SyncType syncType, SapIndicators localEnttity, OICD sapEntity)
        {
            var itemToReturn = new SapIndicators();
            switch (syncType)
            {
                case SyncType.Create:
                    itemToReturn = (SapIndicators)sapEntity.ConvertTo(typeof(SapIndicators));
                    itemToReturn.Code = sapEntity.Code;
                    return itemToReturn;

                case SyncType.Update:
                    itemToReturn.Name = sapEntity.Name;
                    return itemToReturn;

                case SyncType.Delete:
                    return itemToReturn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }

        public IEnumerable<SapIndicators> GetList(string filter)
        {
            var query = DataContext.Context.SapIndicators.AsQueryable();
            return string.IsNullOrEmpty(filter)
                ? query
                : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x =>
                    x.Code.ToLower().Contains(token)
                    || x.Name.ToLower().Contains(token)));
        }
    }
}
