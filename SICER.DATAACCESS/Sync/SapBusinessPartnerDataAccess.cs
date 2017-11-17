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
    public class SapBusinessPartnerDataAccess
    {
        private DataContext DataContext { get; set; }

        public SapBusinessPartnerDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Sync()
        {
            try
            {
                //TODO: ONLY FOR TESTS:
                //var query = new QueryHelper(DataContext.Company?.DbServerType).GetSyncQuery(SyncEntity.OCRD);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OCRD);
                var sapTableType = typeof(OCRD);
                var localTableType = typeof(SapBusinessPartner);

                var localListDefault = DataContext.Context.SapBusinessPartner.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (OCRD)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OCRD>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.CardCode)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                var list = new List<Tuple<SyncType, dynamic>>();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapBusinessPartner.Find(item.CardCode);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.CardCode == item.CardCode);
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

        private SapBusinessPartner ConvertToEntity(SyncType syncType, SapBusinessPartner localEnttity, OCRD sapEntity)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    localEnttity = (SapBusinessPartner)sapEntity.ConvertTo(typeof(SapBusinessPartner));
                    localEnttity.SapBusinessPartnerCardCode = sapEntity.CardCode;
                    break;
                case SyncType.Update:
                    localEnttity.CardName = sapEntity.CardName;
                    localEnttity.CardType = sapEntity.CardType;
                    localEnttity.LictradNum = sapEntity.LictradNum;
                    localEnttity.validFor = sapEntity.validFor;
                    break;
                case SyncType.Delete:
                    break;
                    ;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
            return localEnttity;
        }

        public IEnumerable<SapBusinessPartner> GetList(string filter)
        {
            //TODO: FILTERS
            var query = DataContext.Context.SapBusinessPartner.AsQueryable();
            return string.IsNullOrEmpty(filter)
                ? query
                : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x =>
                    x.CardCode.ToLower().Contains(token)
                    || x.CardName.ToLower().Contains(token)
                    || x.LictradNum.ToLower().Contains(token)));
        }
    }
}
