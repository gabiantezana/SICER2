using SICER.HELPER;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Documento
{
    public class DocumentoDetalleViewModel
    {
        public DocumentoDetalleViewModel()
        {
            this.ListCentrosCosto = new List<JsonEntity>();
            this.ListConceptos = new List<JsonEntity>();
            this.ListMonedas = new List<JsonEntity>();
            this.ListProveedores = new List<JsonEntity>();
            this.ListTipoDocumento = new List<JsonEntity>();
            this.Fecha = DateTime.Now;
        }

        public Int32? IdDocumentoDetalle { get; set; }

        public Int32? TipoDocumento { get; set; }
        public List<JsonEntity> ListTipoDocumento { get; set; }

        [Required]
        public String Serie { get; set; }

        [Required]
        public String Correlativo { get; set; }

        [Required]
        public DateTime? Fecha { get; set; }

        [Required]
        [Display(Name = "Proveedor")]
        public Int32? IdProveedor { get; set; }
        public String Proveedor { get; set; }
        public List<JsonEntity> ListProveedores { get; set; }

        [Required]
        public Int32? IdCentroCosto1 { get; set; }
        public String CentroCosto1 { get; set; }

        public Int32? IdCentroCosto2 { get; set; }
        public String CentroCosto2 { get; set; }

        public Int32? IdCentroCosto3 { get; set; }
        public String CentroCosto3 { get; set; }

        public Int32? IdCentroCosto4 { get; set; }
        public String CentroCosto4 { get; set; }

        public Int32? IdCentroCosto5 { get; set; }
        public String CentroCosto5 { get; set; }

        public List<JsonEntity> ListCentrosCosto { get; set; }

        [Required]
        [Display(Name = "Concepto")]
        public Int32? IdConcepto { get; set; }
        public String Concepto { get; set; }
        public List<JsonEntity> ListConceptos { get; set; }

        [Required]
        [Display(Name = "Moneda original")]
        public Int32? IdMonedaDocumento { get; set; }
        public String MonedaDocumento { get; set; }

        [Required]
        [Display(Name = "Moneda documento")]
        public Int32? IdMonedaDocumentoSAP { get; set; }
        public String MonedaDocumentoSAP { get; set; }
        public List<JsonEntity> ListMonedas { get; set; }

        [Required]
        [Display(Name = "Tasa de cambio")]
        public Decimal? TasaDeCambio { get; set; }

        [Required]
        [Display(Name = "Importe afect")]
        public Decimal? ImporteAfecta { get; set; }

        [Required]
        [Display(Name = "Importe no afecta")]
        public Decimal? ImporteNoAfecta { get; set; }

        [Required]
        [Display(Name = "Importe IGV")]
        public Decimal? ImporteIGV { get; set; }

        [Required]
        [Display(Name = "Importe total")]
        public Decimal? ImporteTotal { get; set; }
    }
}
