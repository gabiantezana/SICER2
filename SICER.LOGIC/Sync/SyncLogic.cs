using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.DATAACCESS.Sync;
using SICER.MODEL;

namespace SICER.LOGIC.Sync
{
    public  class SyncLogic
    {
        private DataContext DataContext { get; set; }

        public SyncLogic(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void SyncAll() //TODO: CALL BY NAVEGADOR
        {
            while (true)
            {

                new SyncDataAccess(DataContext).SyncBusinessPartner();
                new SyncDataAccess(DataContext).SyncCentroCostos();
                new SyncDataAccess(DataContext).SyncConceptos();
                new SyncDataAccess(DataContext).SyncCuentaContables();
                new SyncDataAccess(DataContext).SyncIndicators();
                new SyncDataAccess(DataContext).SyncMonedas();
                new SyncDataAccess(DataContext).SyncTipoCambio();
            }
        }
    }
}
