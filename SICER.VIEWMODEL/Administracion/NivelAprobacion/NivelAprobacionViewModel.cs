using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SICER.HELPER;
using SICER.MODEL;

namespace SICER.VIEWMODEL.Administracion.NivelAprobacion
{
    public class NivelAprobacionViewModel
    {
        public int? NivelAprobacionId { get; set; }

        [Required]
        [DisplayName("Documento")]
        public int TipoDocumentoId { get; set; }
        public IEnumerable<JsonEntity> JListTipoDocumento { get; set; }

        [Required]
        [DisplayName("Número nivel")]
        public int NumeroNivel { get; set; }

        [Required]
        public string Descripcion { get; set; }

    }
}
