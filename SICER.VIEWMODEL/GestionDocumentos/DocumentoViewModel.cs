using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;
using SICER.HELPER;
using SICER.MODEL;

namespace SICER.VIEWMODEL.GestionDocumentos
{
    public class DocumentoViewModel
    {

        #region Propiedades internas

        public SubmitType SubmitType { get; set; }
        public DocumentType DocumentType { get; set; }
        public ModoVistaDocumentoApertura ModoVistaDocumento { get; set; } 
        public IEnumerable<ModoVistaDocumentoApertura> ModosVistaDocumentoList { get; set; }
        public bool MigrateToSap { get; set; } = false;

        public bool UserCanAddRendicion { get; set; } = false;
        public bool UserCanSendRendicion { get; set; }
        public bool UserCanApproveRendicion { get; set; }
        public bool UserCanRefuseRendicion { get; set; }

        #endregion

        #region Propiedades compartidas

        public int? DocumentoId { get; set; }
        public int TipoDocumentoId { get; set; }
        public int? SubTipoDocumento { get; set; }


        [DisplayName("Estado")]
        public int Estado { get; set; }

        [DisplayName("Nivel de aprobación")]
        public int? NivelAprobacion { get; set; }

        [Required]
        [DisplayName("Fecha solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Required]
        [DisplayName("Fecha documento")]
        public DateTime FechaDocumento { get; set; }

        [Required]
        [DisplayName("Fecha contabilización")]
        public DateTime FechaContabilizacion { get; set; }

        [Required]
        [DisplayName("Monto")]
        public decimal Monto { get; set; }

        [DisplayName("Listado de rendiciones")]
        public IEnumerable<DocumentoViewModel> RendicionList { get; set; } = new List<DocumentoViewModel>();

        #endregion

        #region Apertura
        [DisplayName("Código")]
        public string Codigo { get; set; }

        [Required]
        public string Asunto { get; set; }

        public string Motivo { get; set; }

        [DisplayName("Solicitante")]
        public int? CreacionUsuarioid { get; set; }
        public IEnumerable<JsonEntity> UsuarioJList { get; set; } = new List<JsonEntity>();

        [DisplayName("Motivo")]
        public string MotivoRechazo { get; set; }


        #endregion

        #region Rendicion

        public int? AperturaDocumentoId { get; set; }


        public int EntregaRendirDocumentoId { get; set; }

        [Required]
        public string Serie { get; set; }

        [Required]
        public string Correlativo { get; set; }

        #endregion


        #region Campos para SAP

        [Required]
        [DisplayName("Impuesto")]
        public string OstcCode { get; set; }
        public IEnumerable<JsonEntityTwoString> OstcJList { get; set; } = new List<JsonEntityTwoString>();

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

        public string Error { get; set; }

        #endregion


    }
}
