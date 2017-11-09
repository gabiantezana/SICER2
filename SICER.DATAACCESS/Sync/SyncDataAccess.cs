using SICER.MODEL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.HELPER;

namespace SICER.DATAACCESS.Sync
{
    public class SyncDataAccess
    {
        private DataContext DataContext { get; set; }
        public SyncDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        #region SapBusinessPartner

        public void SyncBusinessPartner()
        {
            var query = new QueryHelper(DataContext.SapDbServerType).GetSyncQuery(SyncEntity.BusinessPartner);

            var localList = DataContext.Context.SapBusinessPartner.ToArray();
            var oCrDsaPlist = DataContext.Context.Database.SqlQuery<OCRD>(query).ToArray();
            var oCrdLocalList = localList.Select(x => new OCRD()
            {
                CardCode = x.CardCode,
                CardType = x.CardType,
                LictradNum = x.LictradNum,
                validFor = x.validFor,
                CardName = x.CardName,
            }).ToArray();

            var distinctItemList = oCrDsaPlist.ExceptUsingJSonCompare(oCrdLocalList)
                                                             .Concat(oCrdLocalList.ExceptUsingJSonCompare(oCrDsaPlist))
                                                             .GroupBy(x => x.CardCode)
                                                             .Select(y => y.FirstOrDefault()).ToArray();
            var listToAdd = new List<SapBusinessPartner>();
            var listToUpdate = new List<SapBusinessPartner>();
            var listToDelete = new List<SapBusinessPartner>();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var item in distinctItemList)
            {
                var itemInLocal = localList.FirstOrDefault(x => x.CardCode == item.CardCode);
                if (itemInLocal != null)
                {
                    var itemInSap = oCrDsaPlist.FirstOrDefault(x => x.CardCode == item.CardCode);
                    if (itemInSap != null)
                        listToUpdate.Add(SyncItem(SyncType.Update, itemInLocal, item));
                    else
                        listToDelete.Add(SyncItem(SyncType.Delete, itemInLocal, item));
                }
                else
                    listToAdd.Add(SyncItem(SyncType.Create, null, item));
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            //ADD
            DataContext.Context.Set<SapBusinessPartner>().AddRange(listToAdd);
            DataContext.Context.SaveChanges();

            //UPDATE
            DataContext.Context.Set<SapBusinessPartner>().AddRange(listToUpdate);
            DataContext.Context.ChangeTracker.DetectChanges();
            //DELETE
            DataContext.Context.Set<SapBusinessPartner>().RemoveRange(listToDelete);
            DataContext.Context.SaveChanges();

            //DataContext.Context?.Dispose();
        }

        private SapBusinessPartner SyncItem(SyncType syncType, SapBusinessPartner entity, OCRD ocrd)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    var itemToCreate = (SapBusinessPartner)ocrd.ConvertTo(typeof(SapBusinessPartner));
                    itemToCreate.SapBusinessPartnerCardCode = ocrd.CardCode;
                    return itemToCreate;
                case SyncType.Update:
                    entity.CardCode = ocrd.CardCode;
                    entity.CardName = ocrd.CardName;
                    entity.LictradNum = ocrd.LictradNum;
                    entity.CardType = ocrd.CardType;
                    entity.validFor = ocrd.validFor;
                    return entity;
                case SyncType.Delete:
                    return entity;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }


        #endregion

        #region SapCentroCosto


        #endregion

        #region SapConcepto


        #endregion

        #region SapMoneda


        #endregion

        #region SapTaxCode


        #endregion

        #region SapIndicator


        #endregion
    }
}
