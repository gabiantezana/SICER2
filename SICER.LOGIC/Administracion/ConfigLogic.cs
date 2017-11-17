using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.DATAACCESS.Administracion;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Config;

namespace SICER.LOGIC.Administracion
{
    public class ConfigLogic
    {
        public IPagedList<CONFIG> LstConfigs(DataContext dataContext, String CampoBuscar, Int32? page)
        {
            return new ConfigDataAccess(dataContext).LstConfigs(page);
        }

        public IPagedList<CONFIG> LstConfigs(DataContext dataContext, Int32? page)
        {
            return new ConfigDataAccess(dataContext).LstConfigs(page);
        }

        public List<JsonEntityTwoString> GetConfigs(DataContext dataContext, String CampoBuscar)
        {
            return new ConfigDataAccess(dataContext).GetConfigs(CampoBuscar);
        }

        public IPagedList<CONFIG> GetConfigs(DataContext dataContext, String ConfigId, Int32? p)
        {
            return new ConfigDataAccess(dataContext).GetConfigs(ConfigId, p);
        }

        public ConfigViewModel GetConfig(DataContext dataContext, string ConfigId)
        {
            return new ConfigDataAccess(dataContext).GetConfig(ConfigId);
        }

        public static String GetCONFIGValue(DataContext dataContext, String idConfig)
        {
            return new ConfigDataAccess(dataContext).GetCONFIGValue(idConfig);
        }

        public String SaveConfig(DataContext dataContext, ConfigViewModel model)
        {
            return new ConfigDataAccess(dataContext).SaveConfig(model);
        }

        public bool ValidateConfigRepetido(DataContext dataContext, ConfigViewModel model)
        {
            return new ConfigDataAccess(dataContext).ValidateConfigRepetido(model);
        }

        public void FillListsFromOtherModel(DataContext dataContext, ConfigViewModel fromModel, ref ConfigViewModel model)
        {
            new ConfigDataAccess(dataContext).FillListsFromOtherModel(fromModel, ref model);
        }

    }
}
