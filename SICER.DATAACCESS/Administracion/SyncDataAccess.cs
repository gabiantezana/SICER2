using SICER.HELPER;
using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.DATAACCESS.Administracion
{
    public class SyncDataAccess
    {
        public SyncDataAccess()
        {

        }

        public void SyncTables(DataContext dataContext)
        {
            Sync_SAPProveedor(dataContext);
            Sync_SAPMoneda(dataContext);
            Sync_SAPMetodosPago(dataContext);
            Sync_SAPCentroCostos(dataContext);
            Sync_SAPAccount(dataContext);
        }

        #region Sync_SAPProveedor

        public void Sync_SAPProveedor(DataContext dataContext)
        {
            String query = new QueryHelper(dataContext.SAPDbServerType).QuerySapProveedor();

            List<OCRD> SAPlist = dataContext.Context.Database.SqlQuery<OCRD>(query).ToList();
            List<OCRD> LocalList = dataContext.Context.SAPProveedor
                                                                    .Select(x => new OCRD()
                                                                    {
                                                                        CardName = x.CardName,
                                                                        LictradNum = x.LictradNum,
                                                                        validFor = x.validFor
                                                                    }).ToList();

            List<OCRD> distinctItemList = SAPlist.ExceptUsingJSonCompare(LocalList).Concat(LocalList.ExceptUsingJSonCompare(SAPlist)).ToList();
            distinctItemList = distinctItemList.GroupBy(x => x.LictradNum).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<OCRD, SyncType> ListToSync = new Dictionary<OCRD, SyncType>();
            foreach (var item in distinctItemList)
            {
                Boolean existeEnLocal = LocalList.Where(x => x.LictradNum == item.LictradNum).FirstOrDefault() == null ? false : true;
                if (existeEnLocal)
                {
                    Boolean existeEnSAP = SAPlist.Where(x => x.LictradNum == item.LictradNum).FirstOrDefault() == null ? false : true;
                    if (existeEnSAP)
                        ListToSync.Add(item, SyncType.Update);
                    else
                        ListToSync.Add(item, SyncType.Delete);
                }
                else
                    ListToSync.Add(item, SyncType.Create);
            }

            ListToSync.ToList().ForEach(x => SyncItem(dataContext, x.Key, x.Value));
        }

        public void SyncItem(DataContext dataContext, OCRD ocrd, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    dataContext.Context.SAPProveedor.Add(new SAPProveedor() { CardName = ocrd.CardName, LictradNum = ocrd.LictradNum, validFor = ocrd.validFor });
                    break;
                case SyncType.Update:
                    var item = dataContext.Context.SAPProveedor.FirstOrDefault(x => x.LictradNum == ocrd.LictradNum);
                    item.CardName = ocrd.CardName;
                    item.validFor = ocrd.validFor;
                    dataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = dataContext.Context.SAPProveedor.FirstOrDefault(x => x.LictradNum == ocrd.LictradNum);
                    dataContext.Context.SAPProveedor.Remove(itemToDelete);
                    break;
                default:
                    break;
            }
            dataContext.Context.SaveChanges();
        }

        #endregion

        #region Sync_SAPMoneda

        public void Sync_SAPMoneda(DataContext dataContext)
        {
            String query = new QueryHelper(dataContext.SAPDbServerType).QuerySAPMoneda();

            List<OCRN> SAPlist = dataContext.Context.Database.SqlQuery<OCRN>(query).ToList();
            List<OCRN> LocalList = dataContext.Context.SAPMoneda
                                                                    .Select(x => new OCRN()
                                                                    {
                                                                        CurrName = x.CurrName,
                                                                        DocCurrCod = x.DocCurrCod,
                                                                        Locked = x.Locked
                                                                    }).ToList();

            List<OCRN> distinctItemList = SAPlist.ExceptUsingJSonCompare(LocalList).Concat(LocalList.ExceptUsingJSonCompare(SAPlist)).ToList();
            distinctItemList = distinctItemList.GroupBy(x => x.DocCurrCod).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<OCRN, SyncType> ListToSync = new Dictionary<OCRN, SyncType>();
            foreach (var item in distinctItemList)
            {
                Boolean existeEnLocal = LocalList.Where(x => x.DocCurrCod == item.DocCurrCod).FirstOrDefault() == null ? false : true;
                if (existeEnLocal)
                {
                    Boolean existeEnSAP = SAPlist.Where(x => x.DocCurrCod == item.DocCurrCod).FirstOrDefault() == null ? false : true;
                    if (existeEnSAP)
                        ListToSync.Add(item, SyncType.Update);
                    else
                        ListToSync.Add(item, SyncType.Delete);
                }
                else
                    ListToSync.Add(item, SyncType.Create);
            }

            ListToSync.ToList().ForEach(x => SyncItem(dataContext, x.Key, x.Value));
        }

        public void SyncItem(DataContext dataContext, OCRN item, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    dataContext.Context.SAPMoneda.Add(new SAPMoneda() { DocCurrCod = item.DocCurrCod, CurrName = item.CurrName, Locked = item.Locked });
                    break;
                case SyncType.Update:
                    var _item = dataContext.Context.SAPMoneda.FirstOrDefault(x => x.DocCurrCod == item.DocCurrCod);
                    item.CurrName = item.CurrName;
                    item.Locked = item.Locked;
                    dataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = dataContext.Context.SAPMoneda.FirstOrDefault(x => x.DocCurrCod == item.DocCurrCod);
                    dataContext.Context.SAPMoneda.Remove(itemToDelete);
                    break;
                default:
                    break;
            }
            dataContext.Context.SaveChanges();
        }

        #endregion

        #region Sync_SAPMetodosPago

        public void Sync_SAPMetodosPago(DataContext dataContext)
        {
            String query = new QueryHelper(dataContext.SAPDbServerType).QuerySAPMetodosPago();

            List<OPYM> SAPlist = dataContext.Context.Database.SqlQuery<OPYM>(query).ToList();
            List<OPYM> LocalList = dataContext.Context.SAPMetodoPago
                                                                    .Select(x => new OPYM()
                                                                    {
                                                                        PayMethCod = x.PayMethCod,
                                                                        Active = x.Active,
                                                                        Descript = x.Descript,
                                                                    }).ToList();

            List<OPYM> distinctItemList = SAPlist.ExceptUsingJSonCompare(LocalList).Concat(LocalList.ExceptUsingJSonCompare(SAPlist)).ToList();
            distinctItemList = distinctItemList.GroupBy(x => x.PayMethCod).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<OPYM, SyncType> ListToSync = new Dictionary<OPYM, SyncType>();
            foreach (var item in distinctItemList)
            {
                Boolean existeEnLocal = LocalList.Where(x => x.PayMethCod == item.PayMethCod).FirstOrDefault() == null ? false : true;
                if (existeEnLocal)
                {
                    Boolean existeEnSAP = SAPlist.Where(x => x.PayMethCod == item.PayMethCod).FirstOrDefault() == null ? false : true;
                    if (existeEnSAP)
                        ListToSync.Add(item, SyncType.Update);
                    else
                        ListToSync.Add(item, SyncType.Delete);
                }
                else
                    ListToSync.Add(item, SyncType.Create);
            }

            ListToSync.ToList().ForEach(x => SyncItem(dataContext, x.Key, x.Value));
        }

        public void SyncItem(DataContext dataContext, OPYM item, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    dataContext.Context.SAPMetodoPago.Add(new SAPMetodoPago() { PayMethCod = item.PayMethCod, Descript = item.Descript, Active = item.Active });
                    break;
                case SyncType.Update:
                    var _item = dataContext.Context.SAPMetodoPago.FirstOrDefault(x => x.PayMethCod == item.PayMethCod);
                    _item.Active = item.Active;
                    _item.Descript = item.Descript;
                    _item.PayMethCod = item.PayMethCod;
                    dataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = dataContext.Context.SAPMetodoPago.FirstOrDefault(x => x.PayMethCod == item.PayMethCod);
                    dataContext.Context.SAPMetodoPago.Remove(itemToDelete);
                    break;
                default:
                    break;
            }
            dataContext.Context.SaveChanges();
        }

        #endregion

        #region Sync_SAPCentroCostos

        public void Sync_SAPCentroCostos(DataContext dataContext)
        {
            String query = new QueryHelper(dataContext.SAPDbServerType).QuerySAPCentroCostos();

            List<OPRC> SAPlist = dataContext.Context.Database.SqlQuery<OPRC>(query).ToList();
            List<OPRC> LocalList = dataContext.Context.SAPCentroCostos
                                                                    .Select(x => new OPRC()
                                                                    {
                                                                        Active = x.Active,
                                                                        PrcCode = x.PrcCode,
                                                                        PrcName = x.PrcName,
                                                                    }).ToList();

            List<OPRC> distinctItemList = SAPlist.ExceptUsingJSonCompare(LocalList).Concat(LocalList.ExceptUsingJSonCompare(SAPlist)).ToList();
            distinctItemList = distinctItemList.GroupBy(x => x.PrcCode).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<OPRC, SyncType> ListToSync = new Dictionary<OPRC, SyncType>();
            foreach (var item in distinctItemList)
            {
                Boolean existeEnLocal = LocalList.Where(x => x.PrcCode == item.PrcCode).FirstOrDefault() == null ? false : true;
                if (existeEnLocal)
                {
                    Boolean existeEnSAP = SAPlist.Where(x => x.PrcCode == item.PrcCode).FirstOrDefault() == null ? false : true;
                    if (existeEnSAP)
                        ListToSync.Add(item, SyncType.Update);
                    else
                        ListToSync.Add(item, SyncType.Delete);
                }
                else
                    ListToSync.Add(item, SyncType.Create);
            }

            ListToSync.ToList().ForEach(x => SyncItem(dataContext, x.Key, x.Value));
        }

        public void SyncItem(DataContext dataContext, OPRC item, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    dataContext.Context.SAPCentroCostos.Add(new SAPCentroCostos() { PrcCode = item.PrcCode, PrcName = item.PrcName, Active = item.Active });
                    break;
                case SyncType.Update:
                    var _item = dataContext.Context.SAPCentroCostos.FirstOrDefault(x => x.PrcCode == item.PrcCode);
                    _item.Active = item.Active;
                    _item.PrcCode = item.PrcCode;
                    _item.PrcName = item.PrcName;
                    dataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = dataContext.Context.SAPCentroCostos.FirstOrDefault(x => x.PrcCode == item.PrcCode);
                    dataContext.Context.SAPCentroCostos.Remove(itemToDelete);
                    break;
                default:
                    break;
            }
            dataContext.Context.SaveChanges();
        }

        #endregion

        #region Sync_SAPServicios

        [Obsolete]
        public void Sync_SAPServicio(DataContext dataContext)
        {
            String query = new QueryHelper(dataContext.SAPDbServerType).QuerySAPServicios();

            List<OITM> SAPlist = dataContext.Context.Database.SqlQuery<OITM>(query).ToList();
            List<OITM> LocalList = dataContext.Context.SAPServicio
                                                                    .Select(x => new OITM()
                                                                    {
                                                                        ItemCode = x.itemCode,
                                                                        ItemName = x.itemName,
                                                                        validFor = x.validFor,
                                                                    }).ToList();

            List<OITM> distinctItemList = SAPlist.ExceptUsingJSonCompare(LocalList).Concat(LocalList.ExceptUsingJSonCompare(SAPlist)).ToList();
            distinctItemList = distinctItemList.GroupBy(x => x.ItemCode).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<OITM, SyncType> ListToSync = new Dictionary<OITM, SyncType>();
            foreach (var item in distinctItemList)
            {
                Boolean existeEnLocal = LocalList.Where(x => x.ItemCode == item.ItemCode).FirstOrDefault() == null ? false : true;
                if (existeEnLocal)
                {
                    Boolean existeEnSAP = SAPlist.Where(x => x.ItemCode == item.ItemCode).FirstOrDefault() == null ? false : true;
                    if (existeEnSAP)
                        ListToSync.Add(item, SyncType.Update);
                    else
                        ListToSync.Add(item, SyncType.Delete);
                }
                else
                    ListToSync.Add(item, SyncType.Create);
            }

            ListToSync.ToList().ForEach(x => SyncItem(dataContext, x.Key, x.Value));
        }

        [Obsolete]
        public void SyncItem(DataContext dataContext, OITM item, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    dataContext.Context.SAPServicio.Add(new SAPServicio() { itemCode = item.ItemCode, itemName = item.ItemName, validFor = item.validFor });
                    break;
                case SyncType.Update:
                    var _item = dataContext.Context.SAPServicio.FirstOrDefault(x => x.itemCode == item.ItemCode);
                    _item.itemName = item.ItemName;
                    _item.validFor = item.validFor;
                    dataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = dataContext.Context.SAPServicio.FirstOrDefault(x => x.itemCode == item.ItemCode);
                    dataContext.Context.SAPServicio.Remove(itemToDelete);
                    break;
                default:
                    break;
            }
            dataContext.Context.SaveChanges();
        }

        #endregion

        #region Sync_SAPAccount

        public void Sync_SAPAccount(DataContext dataContext)
        {
            String query = new QueryHelper(dataContext.SAPDbServerType).QuerySAPAccounts();

            List<OACT> SAPlist = dataContext.Context.Database.SqlQuery<OACT>(query).ToList();
            List<OACT> LocalList = dataContext.Context.SAPAccount
                                                                    .Select(x => new OACT()
                                                                    {
                                                                        AcctCode = x.AcctCode,
                                                                        AcctName = x.AcctName,
                                                                        ValidFor = x.ValidFor,
                                                                    }).ToList();

            List<OACT> distinctItemList = SAPlist.ExceptUsingJSonCompare(LocalList).Concat(LocalList.ExceptUsingJSonCompare(SAPlist)).ToList();
            distinctItemList = distinctItemList.GroupBy(x => x.AcctCode).Select(x => x.FirstOrDefault()).ToList();

            Dictionary<OACT, SyncType> ListToSync = new Dictionary<OACT, SyncType>();
            foreach (var item in distinctItemList)
            {
                Boolean existeEnLocal = LocalList.Where(x => x.AcctCode == item.AcctCode).FirstOrDefault() == null ? false : true;
                if (existeEnLocal)
                {
                    Boolean existeEnSAP = SAPlist.Where(x => x.AcctCode == item.AcctCode).FirstOrDefault() == null ? false : true;
                    if (existeEnSAP)
                        ListToSync.Add(item, SyncType.Update);
                    else
                        ListToSync.Add(item, SyncType.Delete);
                }
                else
                    ListToSync.Add(item, SyncType.Create);
            }

            ListToSync.ToList().ForEach(x => SyncItem(dataContext, x.Key, x.Value));
        }

        public void SyncItem(DataContext dataContext, OACT item, SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Create:
                    dataContext.Context.SAPAccount.Add(new SAPAccount() { AcctCode = item.AcctCode, AcctName = item.AcctName, ValidFor = item.ValidFor });
                    break;
                case SyncType.Update:
                    var _item = dataContext.Context.SAPAccount.FirstOrDefault(x => x.AcctCode == item.AcctCode);
                    _item.AcctName = item.AcctName;
                    _item.ValidFor = item.ValidFor;
                    dataContext.Context.Entry(item);
                    break;
                case SyncType.Delete:
                    var itemToDelete = dataContext.Context.SAPAccount.FirstOrDefault(x => x.AcctCode == item.AcctCode);
                    dataContext.Context.SAPAccount.Remove(itemToDelete);
                    break;
                default:
                    break;
            }
            dataContext.Context.SaveChanges();
        }

        #endregion

        public List<SAPCentroCostos> ListSAPCentrosCosto(DataContext dataContext)
        {
            return dataContext.Context.SAPCentroCostos.ToList();
        }

        public List<SAPMetodoPago> ListSAPMetodoPago(DataContext dataContext)
        {
            return dataContext.Context.SAPMetodoPago.ToList();
        }

        public List<SAPMoneda> ListSAPMoneda(DataContext dataContext)
        {
            return dataContext.Context.SAPMoneda.ToList();
        }

        public List<SAPProveedor> ListSAPProveedor(DataContext dataContext)
        {
            return dataContext.Context.SAPProveedor.ToList();
        }

        public List<SAPServicio> ListSAPServicio(DataContext dataContext)
        {
            return dataContext.Context.SAPServicio.ToList();
        }

        public List<SAPAccount> ListSAPAccount(DataContext dataContext)
        {
            return dataContext.Context.SAPAccount.ToList();
        }
    }
}
