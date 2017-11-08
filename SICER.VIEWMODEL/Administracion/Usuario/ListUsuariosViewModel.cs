using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace SICER.VIEWMODEL.Administracion.Usuario
{
   public class ListUsuariosViewModel
    {
        public IPagedList<MODEL.Usuario> ListUsuarios { get; set; }
        public string Filter { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda predictiva
        /// </summary>
        public List<MODEL.Usuario> ListUsuariosDefault { get; set; }
    }
}
