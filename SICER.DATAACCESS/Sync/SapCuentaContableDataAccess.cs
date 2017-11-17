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
    public class SapCuentaContableDataAccess
    {
        private DataContext DataContext { get; set; }

        public SapCuentaContableDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Sync()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.MSS_SICER_CONF);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.MSS_SICER_CONF);
                var sapTableType = typeof(MSS_SICER_AACT);
                var localTableType = typeof(SapCuentaContable);

                var localListDefault = DataContext.Context.SapCuentaContable.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (MSS_SICER_AACT)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<MSS_SICER_AACT>(query).ToArray();

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

        private SapCuentaContable ConvertToEntity(SyncType syncType, SapCuentaContable localEnttity, MSS_SICER_AACT sapEntity)
        {
            var itemToReturn = new SapCuentaContable();
            switch (syncType)
            {
                case SyncType.Create:
                    itemToReturn = (SapCuentaContable)sapEntity.ConvertTo(typeof(SapCuentaContable));
                    return itemToReturn;

                case SyncType.Update:
                    itemToReturn.Name = sapEntity.Name;
                    itemToReturn.U_MSS_ACCT = sapEntity.U_MSS_ACCT;
                    itemToReturn.U_MSS_DSCRPT = sapEntity.U_MSS_DSCRPT;
                    return itemToReturn;

                case SyncType.Delete:
                    return itemToReturn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }

        public IEnumerable<SapCuentaContable> GetList(string filter)
        {
            var query = DataContext.Context.SapCuentaContable.AsQueryable();
            return string.IsNullOrEmpty(filter)
                ? query
                : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x =>
                    x.U_MSS_ACCT.ToLower().Contains(token)
                    || x.U_MSS_DSCRPT.ToLower().Contains(token)));
        }
    }
}
