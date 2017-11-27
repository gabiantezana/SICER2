using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SICER.HELPER;
using SICER.MODEL;

namespace SICER.DATAACCESS.Administracion
{
    public class CompanyDataAccess
    {
        private DataContext DataContext { get; set; }

        public CompanyDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IEnumerable<MODEL.Company> GetList(string filter)
        {
            var query = DataContext.Context.Company.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = filter.ToLower().Split(' ').Aggregate(query, (current, token) => current.Where(x => x.DbName.ToLower().Contains(token)));
            }
            return query;
        }

        public MODEL.Company GetItem(int? CompanyId)
        {
            return DataContext.Context.Company.Find(CompanyId);
        }
    }
}
