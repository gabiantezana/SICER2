using SICER.MODEL;
using System;
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

        public void Start()
        {
            Debug.WriteLine("*********************************************START TO SYNC**************************************");
            while (true)
            {
                SyncBusinessPartner();
                Debug.WriteLine("*********************************************END  SYNC**************************************");
            }
        }

        #region SapBusinessPartner

        public void SyncBusinessPartner()
        {
            var query = new QueryHelper(DataContext.SapDbServerType).GetSyncQuery(SyncEntity.BusinessPartner);

            List<OCRD> saPlist = DataContext.Context.Database.SqlQuery<OCRD>(query)?.ToList();
            List<OCRD> localList = DataContext.Context.SapBusinessPartner.Select(x => x.ConvertTo(typeof(OCRD), false, null) as OCRD).ToList();
            List<OCRD> distinctItemList = saPlist.ExceptUsingJSonCompare(localList).Concat(localList.ExceptUsingJSonCompare(saPlist)).ToList().GroupBy(x => x.CardCode).Select(y => y.FirstOrDefault()).ToList();

            var listToSync = new Dictionary<OCRD, SyncType>();
            foreach (var item in distinctItemList)
            {
                var existeEnLocal = localList.FirstOrDefault(x => x.CardCode == item.CardCode) != null;
                if (existeEnLocal)
                {
                    var existeEnSap = saPlist.FirstOrDefault(x => x.CardCode == item.CardCode) != null;
                    listToSync.Add(item, existeEnSap ? SyncType.Update : SyncType.Delete);
                }
                else
                    listToSync.Add(item, SyncType.Create);
            }

            listToSync.ToList().ForEach(x => SyncItem(x.Key, x.Value));
        }

        private void SyncItem(OCRD ocrd, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    var itemToCreate = (SapBusinessPartner)ocrd.ConvertTo(typeof(SapBusinessPartner));
                    itemToCreate.SapBusinessPartnerCardCode = ocrd.CardCode;
                    DataContext.Context.SapBusinessPartner.Add(itemToCreate);
                    break;
                case SyncType.Update:
                    var item = DataContext.Context.SapBusinessPartner.Find(ocrd.CardCode);
                    item.CardCode = ocrd.CardCode;
                    item.CardName = ocrd.CardName;
                    item.LictradNum = ocrd.LictradNum;
                    item.CardType = ocrd.CardType;
                    item.validFor = ocrd.validFor;
                    DataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = DataContext.Context.SapBusinessPartner.Find(ocrd.CardCode);
                    DataContext.Context.SapBusinessPartner.Remove(itemToDelete);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
            DataContext.Context.SaveChanges();
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
