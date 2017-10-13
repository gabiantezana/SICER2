using SICER.HELPER;
using SICER.MODEL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.VIEWMODEL.Administracion.Documento
{
    public class DocumentoViewModel
    {
        public DocumentoViewModel()
        {
            this.ListDetalle = new List<DocumentoDetalleViewModel>();
        }

        [Required]
        public String Asunto { get; set; }

        [Required]
        public String Motivo { get; set; }

        [Display(Name = "Fecha")]
        public DateTime? FechaDocumento { get; set; }

        [Display(Name = "Tipo documento")]
        public DocumentType DocumentType { get; set; }


        [Display(Name = "Id Documento")]
        public Int32? IdDocumento { get; set; }
     

        [Required]
        [Display(Name = "Proveedor")]
        public Int32? IdProveedorSolicitante { get; set; }
        public String ProveedorSolicitante { get; set; }
        public List<JsonEntity> ListProveedorSolicitante { get; set; }

        [Required]
        [Display(Name = "Moneda")]
        public Int32? IdMoneda { get; set; }
        public String Moneda { get; set; }
        public List<JsonEntity> ListMonedas { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public Int32? IdMetodoPago { get; set; }
        public String MetodoPago { get; set; }
        public List<JsonEntity> ListMetodosPago { get; set; }
     

        [Required]
        [Display(Name = "Centro de costos 1")]
        public Int32? IdCentroCosto1 { get; set; }
        public String CentroCosto1 { get; set; }

        [Display(Name = "Centro de costos 2")]
        public Int32? IdCentroCosto2 { get; set; }
        public String CentroCosto2 { get; set; }

        [Display(Name = "Centro de costos 3")]
        public Int32? IdCentroCosto3 { get; set; }
        public String CentroCosto3 { get; set; }

        [Display(Name = "Centro de costos 4")]
        public Int32? IdCentroCosto4 { get; set; }
        public String CentroCosto4 { get; set; }

        [Display(Name = "Centro de costos 5")]
        public Int32? IdCentroCosto5 { get; set; }
        public String CentroCosto5 { get; set; }

        [Display(Name = "Centro de costos 6")]
        public Int32? IdCentroCosto6 { get; set; }
        public String CentroCosto6 { get; set; }

        public List<JsonEntity> ListCentrosCosto { get; set; }

     

        public String Observaciones { get; set; }

        [Display(Name = "Estado documento")]
        public DocumentState DocumentState { get; set; }
        public String DocumentStateString { get; set; }

        public List<DocumentoDetalleViewModel> ListDetalle { get; set; }
    }
}
