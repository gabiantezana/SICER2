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
    public class SapMonedaDataAccess
    {
        private DataContext DataContext { get; set; }
        public SapMonedaDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }
        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OCRN, companyEntity);
                var sapTableType = typeof(OCRN);
                var localTableType = typeof(SapMoneda);

                var localListDefault = DataContext.Context.SapMoneda.ToArray();
                var localList = localListDefault.Select(x => new OCRN()
                {
                    CurrName = x.CurrName,
                    Locked = x.Locked,
                    DocCurrCod = x.SapMonedaDocCurrCod
                }).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OCRN>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.DocCurrCod)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapMoneda.Find(item.DocCurrCod);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.DocCurrCod == item.DocCurrCod);
                        if (itemInSap != null)
                        {
                            DataContext.Context.Entry(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                        else
                        {
                            DataContext.Context.SapMoneda.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.SapMoneda.Add(new SapMoneda()
                        {
                            SapMonedaDocCurrCod = item.DocCurrCod,
                            CurrName = item.CurrName,
                            Locked = item.Locked,
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
        private SapMoneda ConvertToEntity(SyncType syncType, SapMoneda localEnttity, OCRN sapEntity)
        {
            var itemToReturn = new SapMoneda();
            switch (syncType)
            {
                case SyncType.Create:
                    itemToReturn = (SapMoneda)sapEntity.ConvertTo(typeof(SapMoneda));
                    itemToReturn.SapMonedaDocCurrCod = sapEntity.DocCurrCod;
                    return itemToReturn;

                case SyncType.Update:
                    itemToReturn.CurrName = sapEntity.CurrName;
                    itemToReturn.Locked = sapEntity.Locked;
                    return itemToReturn;

                case SyncType.Delete:
                    return itemToReturn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }

        public IEnumerable<SapMoneda> GetList(string filter)
        {
            var query = DataContext.Context.SapMoneda.AsQueryable();
            return string.IsNullOrEmpty(filter)
                ? query
                : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x =>
                    x.SapMonedaDocCurrCod.ToLower().Contains(token)
                    || x.CurrName.ToLower().Contains(token)));
        }
    }

}
