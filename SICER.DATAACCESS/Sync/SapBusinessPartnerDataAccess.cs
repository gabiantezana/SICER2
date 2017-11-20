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

        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OCRD, companyEntity);
                var sapTableType = typeof(OCRD);
                var localTableType = typeof(SapBusinessPartner);

                var localListDefault = DataContext.Context.SapBusinessPartner.ToArray();

                var localList = localListDefault.Select(x => new OCRD()
                {
                    CardCode = x.CardCode,
                    LictradNum = x.LictradNum,
                    CardName = x.CardName,
                    CardType = x.CardType,
                    validFor = x.validFor
                }).ToList();

                var sapList = DataContext.Context.Database.SqlQuery<OCRD>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.CardCode)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapBusinessPartner.Find(item.CardCode);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.CardCode == item.CardCode);
                        if (itemInSap != null)
                        {
                            itemInLocal.CardName = itemInSap.CardName;
                            itemInLocal.CardType = itemInSap.CardType;
                            itemInLocal.LictradNum = itemInSap.LictradNum;

                            DataContext.Context.Entry(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                        else
                        {
                            DataContext.Context.SapBusinessPartner.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.SapBusinessPartner.Add(new SapBusinessPartner()
                        {
                            SapBusinessPartnerCardCode = item.CardCode,
                            CardCode = item.CardCode,
                            CardName = item.CardName,
                            CardType = item.CardType,
                            LictradNum = item.LictradNum,
                            validFor = item.validFor,
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
