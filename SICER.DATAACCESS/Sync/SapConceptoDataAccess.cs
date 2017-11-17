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
    public class SapConceptoDataAccess
    {
        private DataContext DataContext { get; set; }
        public SapConceptoDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }
        public void Sync()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.MSS_SICER_CCPT);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.MSS_SICER_CCPT);
                var sapTableType = typeof(OOCR);
                var localTableType = typeof(SapConcepto);

                var localListDefault = DataContext.Context.SapConcepto.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (MSS_SICER_CCPT)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<MSS_SICER_CCPT>(query).ToArray();

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

        private SapConcepto ConvertToEntity(SyncType syncType, SapConcepto localEnttity, MSS_SICER_CCPT sapEntity)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    localEnttity = (SapConcepto)sapEntity.ConvertTo(typeof(SapConcepto));
                    break;
                case SyncType.Update:
                    localEnttity.Name = sapEntity.Name;
                    localEnttity.U_MSS_ACCT = sapEntity.U_MSS_ACCT;
                    localEnttity.U_MSS_DSCRPT = sapEntity.U_MSS_ACCT;
                    break;
                case SyncType.Delete:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
            return localEnttity;
        }

        public IEnumerable<SapConcepto> GetList(string filter)
        {
            var query = DataContext.Context.SapConcepto.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x => x.U_MSS_ACCT.ToLower().Contains(token) || x.U_MSS_DSCRPT.ToLower().Contains(token)));
            }
            return query;
        }
    }
}
