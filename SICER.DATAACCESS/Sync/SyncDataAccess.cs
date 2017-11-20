using SICER.MODEL;
using System;
using System.CodeDom;
using System.Collections;
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

        #region SAVE TO DATABASE

        public void SaveToDb(Type entityType, IReadOnlyCollection<Tuple<SyncType, dynamic>> tupleList)
        {
            DataContext.Context = new SICEREntities();
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
