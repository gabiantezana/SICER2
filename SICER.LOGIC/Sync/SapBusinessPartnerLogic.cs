using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using SICER.DATAACCESS.Sync;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Sync;

namespace SICER.LOGIC.Sync
{
    public static class SapBusinessPartnerLogic
    {
        public static ListSapBusinessPartnerViewModel GetListViewModel(DataContext dataContext)
        {
            return new ListSapBusinessPartnerViewModel()
            {
                PagedList = GetPagedList(dataContext, null, 1),
                ListDefault = new List<SapBusinessPartner>(),
                Filter = string.Empty
            };
        }

        public static IEnumerable<SapBusinessPartner> GetList(DataContext dataContext, string filter)
        {
            return new SapBusinessPartnerDataAccess(dataContext).GetList(filter).ToList();
        }

        public static IPagedList<SapBusinessPartner> GetPagedList(DataContext dataContext, string filter, int? page)
        {
            return GetList(dataContext, filter).ToPagedList(page ?? 1,ConstantHelper.NUMEROFILASPORPAGINA);
        }

        public static List<JsonEntityTwoString> GetJList(DataContext dataContext, string filter)
        {
            return GetList(dataContext, filter).Select(x => new JsonEntityTwoString()
            {
                id = x.SapBusinessPartnerCardCode,
                text = x.LictradNum + ConstantHelper.SEPARADOR_NOMBRE_DESCRIPCION_SELECT + x.CardName,
            }).ToList();
        }
    }
}
