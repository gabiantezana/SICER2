using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SICER.HELPER;

namespace SICER.VIEWMODEL.GestionDocumentos
{
    public class DocumentoViewModel
    {
        public DocumentType DocumentType { get; set; }

        public int? DocumentoId { get; set; }
        public int AperturaDocumentoId { get; set; }
        public int EntregaRendirDocumentoId { get; set; }


        [DisplayName("Solicitante")]
        public int? CreacionUsuarioid { get; set; }
        public IEnumerable<JsonEntity> UsuarioJList { get; set; } = new List<JsonEntity>();


        public List<DocumentoViewModel> RendicionList { get; set; } = new List<DocumentoViewModel>();

        public ModoVistaDocumento ModoVistaDocumento { get; set; } = ModoVistaDocumento.Ver;

        [Required]
        public int TipoDocumentoId { get; set; }
        //public IEnumerable<JsonEntity> TipoDocumentoJList { get; set; } = new List<JsonEntity>();

        public int? SubTipoDocumento { get; set; }
        public IEnumerable<JsonEntity> SubTipoDocumentoJList { get; set; } = new List<JsonEntity>();

        [DisplayName("Nivel de aprobación")]
        public int? NivelAprobacion { get; set; }

        [Required]
        [DisplayName("Tipo documento")]
        public string SapIndicatorCode { get; set; }
        public IEnumerable<JsonEntityTwoString> SapIndicatorJList { get; set; } = new List<JsonEntityTwoString>();

        [Required]
        [DisplayName("Proveedor")]
        public string SapBusinessPartnerCardCode { get; set; }
        public IEnumerable<JsonEntityTwoString> SapBusinessPartnerJList { get; set; } = new List<JsonEntityTwoString>();

        [Required]
        [DisplayName("Moneda")]
        public string SapMonedaDocCurrCode { get; set; }
        public IEnumerable<JsonEntityTwoString> SapMonedaJList { get; set; } = new List<JsonEntityTwoString>();

        [Required]
        [DisplayName("Cuenta contable")]
        public string AsociadaSapCuentaContableCode { get; set; }
        public IEnumerable<JsonEntityTwoString> AsociadaSapCuentaContableJList { get; set; } = new List<JsonEntityTwoString>();

        public string GastoSapCuentaContableCode { get; set; }
        public IEnumerable<JsonEntityTwoString> GastoSapCuentaContableJList{ get; set; } = new List<JsonEntityTwoString>();

        [DisplayName("Concepto")]
        public string SapConceptoCode { get; set; }
        public IEnumerable<JsonEntityTwoString> SapConceptoJList { get; set; } = new List<JsonEntityTwoString>();

        [Required]
        [DisplayName("Centro costo 1")]
        public string C_1SapCentroCostoOcrCode { get; set; }
        public IEnumerable<JsonEntityTwoString> C_1SapCentroCostoJList { get; set; } = new List<JsonEntityTwoString>();

        [DisplayName("Centro costo 2")]
        public string C_2SapCentroCostoOcrCode { get; set; }
        public IEnumerable<JsonEntityTwoString> C_2SapCentroCostoJList { get; set; } = new List<JsonEntityTwoString>();

        [DisplayName("Centro costo 3")]
        public string C_3SapCentroCostoOcrCode { get; set; }
        public IEnumerable<JsonEntityTwoString> C_3SapCentroCostoJList { get; set; } = new List<JsonEntityTwoString>();

        [DisplayName("Centro costo 4")]
        public string C_4SapCentroCostoOcrCode { get; set; }
        public IEnumerable<JsonEntityTwoString> C_4SapCentroCostoJList { get; set; } = new List<JsonEntityTwoString>();

        [DisplayName("Centro costo 5")]
        public string C_5SapCentroCostoOcrCode { get; set; }
        public IEnumerable<JsonEntityTwoString> C_5SapCentroCostoJList { get; set; } = new List<JsonEntityTwoString>();

        public int Estado { get; set; }

        public string Codigo { get; set; }

        [Required]
        public string Asunto { get; set; }

        public string Motivo { get; set; }

        [Required]
        public string Serie { get; set; }

        [Required]
        public string Correlativo { get; set; }

        [Required]
        [DisplayName("Fecha solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Required]
        [DisplayName("Fecha documento")]
        public DateTime FechaDocumento { get; set; }

        [DisplayName("Fecha contabilización")]
        public DateTime FechaContabilizacion { get; set; }

        [Required]
        [DisplayName("Monto afecto")]
        public decimal MontoAfecto { get; set; }

        [Required]
        [DisplayName("Monto no afecto")]
        public decimal MontoNoAfecto { get; set; }

        [Required]
        [DisplayName("Monto")]
        public decimal Monto { get; set; }

        [Required]
        [DisplayName("Monto IGV")]
        public decimal MontoIgv { get; set; }

        public string MotivoRechazo { get; set; }

        public decimal MontoTotal { get; set; }

        public bool MigrateToSap { get; set; } = false;

    }
}
