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

        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.MSS_SICER_CONF, companyEntity);
                var sapTableType = typeof(MSS_SICER_AACT);
                var localTableType = typeof(SapCuentaContable);

                var localListDefault = DataContext.Context.SapCuentaContable.ToArray();
                var localList = localListDefault.Select(x => new MSS_SICER_AACT()
                {
                    Name = x.Name,
                    Code = x.Code,
                    U_MSS_ACC = x.U_MSS_ACC,
                    U_MSS_DSC = x.U_MSS_DSC,
                    U_MSS_IBA = x.U_MSS_IBA
                }).ToList();

                var sapList = DataContext.Context.Database.SqlQuery<MSS_SICER_AACT>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.Code)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapCuentaContable.Find(item.Code);
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
                            DataContext.Context.SapCuentaContable.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.SapCuentaContable.Add(new SapCuentaContable()
                        {
                            Code = item.Code,
                            Name = item.Name,
                            U_MSS_ACC = item.U_MSS_ACC,
                            U_MSS_DSC = item.U_MSS_DSC,
                            U_MSS_IBA = item.U_MSS_IBA,
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
                    itemToReturn.U_MSS_ACC = sapEntity.U_MSS_ACC;
                    itemToReturn.U_MSS_DSC = sapEntity.U_MSS_DSC;
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
                    x.U_MSS_ACC.ToLower().Contains(token)
                    || x.U_MSS_DSC.ToLower().Contains(token)));
        }
    }
}
