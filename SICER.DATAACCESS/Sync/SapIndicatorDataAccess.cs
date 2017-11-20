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

        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.OICD);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OICD, companyEntity);
                var sapTableType = typeof(OICD);
                var localTableType = typeof(SapIndicators);

                var localListDefault = DataContext.Context.SapIndicators.ToArray();
                var localList = localListDefault.Select(x => new OICD()
                {
                    Name = x.Name,
                    Code = x.Code
                }).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OICD>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.Code)
                                                    .Select(y => y.FirstOrDefault()).ToArray();


                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapIndicators.Find(item.Code);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.Code == item.Code);
                        if (itemInSap != null)
                        {
                            DataContext.Context.Entry(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                        else
                        {
                            DataContext.Context.SapIndicators.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.SapIndicators.Add(new SapIndicators()
                        {
                            Code = item.Code,
                            Name = item.Name,
                        });

                        DataContext.Context.SaveChanges();
                    }
                }
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
