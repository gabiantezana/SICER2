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
    public class SapCentroCostoDataAccess
    {
        private DataContext DataContext { get; set; }

        public SapCentroCostoDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void Sync(CompanyEntity companyEntity)
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.OOCR);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OOCR, companyEntity);
                var sapTableType = typeof(OOCR);
                var localTableType = typeof(SapCentroCosto);

                var localListDefault = DataContext.Context.SapCentroCosto.ToArray();
                var localList = localListDefault.Select(x => new OOCR()
                {
                    OcrCode = x.OcrCode,
                    OcrName = x.OcrName,
                    Locked = x.Locked
                }).ToList(); 

                var sapList = DataContext.Context.Database.SqlQuery<OOCR>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.OcrCode)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = DataContext.Context.SapCentroCosto.Find(item.DimCode);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.DimCode == item.DimCode);
                        if (itemInSap != null)
                        {
                            DataContext.Context.Entry(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                        else
                        {
                            DataContext.Context.SapCentroCosto.Remove(itemInLocal);
                            DataContext.Context.SaveChanges();
                        }
                    }
                    else
                    {
                        DataContext.Context.SapCentroCosto.Add(new SapCentroCosto()
                        {
                          OcrCode = item.OcrCode,
                          OcrName = item.OcrName,
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

        private SapCentroCosto ConvertToEntity(SyncType syncType, SapCentroCosto localEnttity, OOCR sapEntity)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    localEnttity = (SapCentroCosto)sapEntity.ConvertTo(typeof(SapCentroCosto));
                    localEnttity.SapCentroCostoOcrCode = sapEntity.OcrCode;
                    return localEnttity;

                case SyncType.Update:
                    localEnttity.DimCode = sapEntity.DimCode;
                    localEnttity.Locked = sapEntity.Locked;
                    localEnttity.OcrName = sapEntity.OcrName;
                    return localEnttity;

                case SyncType.Delete:
                    return localEnttity;

                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }

        public IEnumerable<SapCentroCosto> GetList(int? dimCode, string filter)
        {
            var query = DataContext.Context.SapCentroCosto.AsQueryable();

            if (dimCode.HasValue) query = query.Where(x => x.DimCode == dimCode);
            return string.IsNullOrEmpty(filter) ? query : filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x => x.OcrCode.Contains(token) || x.OcrName.Contains(token)));
        }
    }
}
