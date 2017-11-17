using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SICER.MODEL;

namespace SICER.VIEWMODEL.Administracion.Config
{
  public  class LstConfigViewModel
    {
        public IPagedList<CONFIG> LstConfigs { get; set; }
        public String CampoBuscar { get; set; }
        public List<CONFIG> LstConfigsDefault { get; set; }
    }
}
