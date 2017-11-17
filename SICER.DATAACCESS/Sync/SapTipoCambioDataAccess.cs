using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SAPbobsCOM;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.DATAACCESS.Sync
{
   public class SapTipoCambioDataAccess
    {
        private DataContext DataContext { get; set; }

        public SapTipoCambioDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Sync()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.asdf).GetSyncQuery(SyncEntity.ORTT);
                DataContext.Context = new SICEREntities();
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.ORTT);
                var localTableType = typeof(SapTipoCambio);
                var sapList = DataContext.Context.Database.SqlQuery<ORTT>(query).ToArray();
                var list = sapList.Select(item => new Tuple<SyncType, dynamic>(SyncType.Create, ConvertToEntity(SyncType.Create, new SapTipoCambio(), item))).ToList();
                using (var transaction = new TransactionScope())
                {
                    DataContext.Context.SapTipoCambio.RemoveRange(DataContext.Context.SapTipoCambio);
                    new SyncDataAccess(DataContext).SaveToDb(localTableType, list);
                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        private SapTipoCambio ConvertToEntity(SyncType syncType, SapTipoCambio localEnttity, ORTT sapEntity)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    localEnttity = (SapTipoCambio)sapEntity.ConvertTo(typeof(SapTipoCambio));
                    localEnttity.Currency = sapEntity.Currency;
                    localEnttity.RateDate = sapEntity.RateDate;
                    localEnttity.Rate = sapEntity.Rate;
                    return localEnttity;
                case SyncType.Update:
                    return localEnttity;

                case SyncType.Delete:
                    return localEnttity;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }

        public IEnumerable<SapTipoCambio> GetList(string filter)
        {
            var query = DataContext.Context.SapTipoCambio.AsQueryable();
            return string.IsNullOrEmpty(filter)
                ? query
                : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x =>
                    x.Currency.ToLower().Contains(token)
                    || x.RateDate.ToString().ToLower().Contains(token)));
        }

    }
}
