using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICER.Controllers;
using SICER.LOGIC.Sync;

namespace SICER.Areas.Sync.Controllers
{
    public class SyncController : BaseController
    {
        public void SyncDataBase()
        {
            new SyncLogic(GetDataContext()).SyncAll();
        }
    }
}