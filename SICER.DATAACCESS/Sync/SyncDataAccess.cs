using SICER.MODEL;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SAPbobsCOM;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODELCODEFIRST;
using SICER.SAPUSERMODEL.Tables;

namespace SICER.DATAACCESS.Sync
{
    public class SyncDataAccess
    {
        private DataContext DataContext { get; set; }

        public SyncDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        #region  Sync Entities

        public void SyncBusinessPartner()
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

                SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public void SyncCentroCostos()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.OOCR);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OOCR);
                var sapTableType = typeof(OOCR);
                var localTableType = typeof(SapCentroCosto);

                var localListDefault = DataContext.Context.SapCentroCosto.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (OOCR)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OOCR>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.OcrCode)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                var list = new List<Tuple<SyncType, dynamic>>();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = localListDefault.FirstOrDefault(x => x.OcrCode == item.OcrCode);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.OcrCode == item.OcrCode);
                        list.Add(itemInSap != null
                            ? new Tuple<SyncType, dynamic>(SyncType.Update, ConvertToEntity(SyncType.Update, itemInLocal, item))
                            : new Tuple<SyncType, dynamic>(SyncType.Delete, ConvertToEntity(SyncType.Delete, itemInLocal, item)));
                    }
                    else
                        list.Add(new Tuple<SyncType, dynamic>(SyncType.Create, ConvertToEntity(SyncType.Create, itemInLocal, item)));
                }

                SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public void SyncConceptos()
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

                SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public void SyncCuentaContables()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.MSS_SICER_CONF);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.MSS_SICER_CONF);
                var sapTableType = typeof(MSS_SICER_AACT);
                var localTableType = typeof(SapCuentaContable);

                var localListDefault = DataContext.Context.SapCuentaContable.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (MSS_SICER_AACT)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<MSS_SICER_AACT>(query).ToArray();

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

                SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public void SyncIndicators()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.OICD);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OICD);
                var sapTableType = typeof(OICD);
                var localTableType = typeof(SapIndicators);

                var localListDefault = DataContext.Context.SapIndicators.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (OICD)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OICD>(query).ToArray();

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

                SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public void SyncMonedas()
        {
            try
            {
                //var query = new QueryHelper(DataContext.Company.DbServerType).GetSyncQuery(SyncEntity.OCRN);
                var query = new QueryHelper(BoDataServerTypes.dst_MSSQL).GetSyncQuery(SyncEntity.OCRN);
                var sapTableType = typeof(OCRN);
                var localTableType = typeof(SapMoneda);

                var localListDefault = DataContext.Context.SapMoneda.ToArray();
                var localList = localListDefault.Select(item => item.ConvertTo(sapTableType))
                                                    .Select(dummy => (OCRN)dummy).ToList();
                var sapList = DataContext.Context.Database.SqlQuery<OCRN>(query).ToArray();

                var pendingToSyncList = sapList.ExceptUsingJSonCompare(localList)
                                                    .Concat(localList.ExceptUsingJSonCompare(sapList))
                                                    .GroupBy(x => x.DocCurrCod)
                                                    .Select(y => y.FirstOrDefault()).ToArray();

                var list = new List<Tuple<SyncType, dynamic>>();

                foreach (var item in pendingToSyncList)
                {
                    var itemInLocal = localListDefault.FirstOrDefault(x => x.SapMonedaDocCurrCod == item.DocCurrCod);
                    if (itemInLocal != null)
                    {
                        var itemInSap = sapList.FirstOrDefault(x => x.DocCurrCod == item.DocCurrCod);
                        list.Add(itemInSap != null
                            ? new Tuple<SyncType, dynamic>(SyncType.Update, ConvertToEntity(SyncType.Update, itemInLocal, item))
                            : new Tuple<SyncType, dynamic>(SyncType.Delete, ConvertToEntity(SyncType.Delete, itemInLocal, item)));
                    }
                    else
                        list.Add(new Tuple<SyncType, dynamic>(SyncType.Create, ConvertToEntity(SyncType.Create, itemInLocal, item)));
                }

                SaveToDb(localTableType, list);
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        public void SyncTipoCambio()
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
                    SaveToDb(localTableType, list);
                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                ExceptionHelper.LogException(e, DataContext);
            }
        }

        #endregion

        #region Convert Sap Entity to Local ToEntity 

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

        private SapCuentaContable ConvertToEntity(SyncType syncType, SapCuentaContable localEnttity, MSS_SICER_AACT sapEntity)
        {
            var itemToReturn = new SapCuentaContable();
            switch (syncType)
            {
                case SyncType.Create:
                    itemToReturn = (SapCuentaContable)sapEntity.ConvertTo(typeof(SapCuentaContable));
                    return itemToReturn;

                case SyncType.Update:
                    itemToReturn.Name = sapEntity.Name;
                    itemToReturn.U_MSS_ACCT = sapEntity.U_MSS_ACCT;
                    itemToReturn.U_MSS_DSCRPT = sapEntity.U_MSS_DSCRPT;
                    return itemToReturn;

                case SyncType.Delete:
                    return itemToReturn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
            }
        }

        private SapIndicators ConvertToEntity(SyncType syncType, SapIndicators localEnttity, OICD sapEntity)
        {
            var itemToReturn = new SapIndicators();
            switch (syncType)
            {
                case SyncType.Create:
                    itemToReturn = (SapIndicators)sapEntity.ConvertTo(typeof(SapIndicators));
                    itemToReturn.Code = sapEntity.Code;
                    return itemToReturn;

                case SyncType.Update:
                    itemToReturn.Name = sapEntity.Name;
                    return itemToReturn;

                case SyncType.Delete:
                    return itemToReturn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncType), syncType, null);
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

        #endregion

        #region SAVE TO DATABASE


        private void SaveToDb(Type entityType, IReadOnlyCollection<Tuple<SyncType, dynamic>> tupleList)
        {
            //DELETE
            var listToDelete = tupleList.Where(x => x.Item1 == SyncType.Delete).Select(x => x.Item2).ToList();
            if (listToDelete.Any())
            {
                DataContext.Context.Set(entityType).RemoveRange(listToDelete);
                DataContext.Context.SaveChanges();
            }

            //UPDATE
            var listToUpdate = tupleList.Where(x => x.Item1 == SyncType.Update).Select(x => x.Item2).ToList();
            if (listToUpdate.Any())
            {
                DataContext.Context.Set(entityType).AddRange(listToUpdate);
                DataContext.Context.ChangeTracker.DetectChanges();
                DataContext.Context.SaveChanges();
            }

            //ADD
            var listToCreate = tupleList.Where(x => x.Item1 == SyncType.Create).Select(x => x.Item2).ToList();
            if (!listToCreate.Any()) return;
            DataContext.Context.Set(entityType).AddRange(listToCreate);
            DataContext.Context.SaveChanges();

            //TODO: REFRESH CONTEXT
        }

        #endregion

    }
}
