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
        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.MSS_SICER_CCPT, companyEntity);
                var sapTableType = typeof(OOCR);
                var localTableType = typeof(SapConcepto);

                var localListDefault = DataContext.Context.SapConcepto.ToArray();
                var localList = localListDefault.Select(x => new MSS_SICER_CCPT()
                {
                    Name = x.Name,
                    Code =  x.Code,
                    U_MSS_ACC = x.U_MSS_ACC,
                    U_MSS_DSC = x.U_MSS_ACC
                });

                var sapList = DataContext.Context.Database.SqlQuery<MSS_SICER_CCPT>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.Code)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapConcepto.Find(item.Code);
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
                            DataContext.Context.SapConcepto.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.SapConcepto.Add(new SapConcepto()
                        {
                            Code = item.Code,
                            Name = item.Name,
                            U_MSS_ACC = item.U_MSS_ACC,
                            U_MSS_DSC = item.U_MSS_DSC
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

        private SapConcepto ConvertToEntity(SyncType syncType, SapConcepto localEnttity, MSS_SICER_CCPT sapEntity)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    localEnttity = (SapConcepto)sapEntity.ConvertTo(typeof(SapConcepto));
                    break;
                case SyncType.Update:
                    localEnttity.Name = sapEntity.Name;
                    localEnttity.U_MSS_ACC= sapEntity.U_MSS_ACC;
                    localEnttity.U_MSS_DSC = sapEntity.U_MSS_ACC;
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
                query = filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x => x.U_MSS_ACC.ToLower().Contains(token) || x.U_MSS_DSC.ToLower().Contains(token)));
            }
            return query;
        }
    }
}
