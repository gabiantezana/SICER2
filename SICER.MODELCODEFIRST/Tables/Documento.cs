using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class Documento
    {
        public Documento() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentoId { get; set; }
        public string Codigo { get; set; }
        public string Asunto { get; set; }
        public string Motivo { get; set; }
        public int NumeroRendicion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public Double MontoInicial { get; set; }

        #region Foreign Keys

        public int TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }

        public string SapBusinessPartnerCardCode { get; set; }
        public SapBusinessPartner SapBusinessPartner { get; set; }

        public string _1SapCentroCostoOcrCode { get; set; }
        public SapCentroCosto _1SAPCentroCosto { get; set; }

        public string _2SapCentroCostoOcrCode { get; set; }
        public SapCentroCosto _2SAPCentroCosto { get; set; }

        public string _3SapCentroCostoOcrCode { get; set; }
        public SapCentroCosto _3SAPCentroCosto { get; set; }

        public string _4SapCentroCostoOcrCode { get; set; }
        public SapCentroCosto _4SAPCentroCosto { get; set; }

        public string _5SapCentroCostoOcrCode { get; set; }
        public SapCentroCosto _5SAPCentroCosto { get; set; }

        public int SapMetodoPagoId { get; set; }
        public SapMetodoPago SAPMetodoPAgo { get; set; }

        public int TipoDocumentoSunatId { get; set; }
        public virtual TipoDocumentoSunat TipoDocumentoSunat { get; set; }

        #endregion

        #region Lists

        public virtual ICollection<DocumentoRendicion> DocumentoRendicions { get; set; }

        #endregion
    }
}
