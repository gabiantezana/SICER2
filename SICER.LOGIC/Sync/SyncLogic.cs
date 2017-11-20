using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.DATAACCESS.Sync;
using SICER.MODEL;

namespace SICER.LOGIC.Sync
{
    public class SyncLogic
    {
        private DataContext DataContext { get; set; }

        public SyncLogic(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void SyncAll()
        {
            new SapBusinessPartnerDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new SapCentroCostoDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new SapConceptoDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new SapCuentaContableDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new SapIndicatorDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new SapMonedaDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new SapTipoCambioDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
            new OstcDataAccess(DataContext).Sync(new SapLogic().GetCompanyEntityFromFile());
        }
    }
}
