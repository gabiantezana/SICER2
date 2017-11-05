using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SICER.MODELCODEFIRST.Tables
{
    public class DocumentoRendicion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentoRendicionId { get; set; }

        public int NumeroRendicion { get; set; }
        public string Serie { get; set; }
        public string Correlativo { get; set; }
        public double MontoDocumento { get; set; }
        public double MontoTasaCambio { get; set; }
        public double MontoNoAfecto { get; set; }
        public double MontoAfecto { get; set; }
        public double MontoIgv { get; set; }
        public double MontoTotal { get; set; }
        public int EstadoRendicion { get; set; }
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        #region Foreign Keys

        public int DocumentoId { get; set; }
        public virtual Documento Documento { get; set; }

        public string SapBusinessPartnerCardCode { get; set; }
        public virtual SapBusinessPartner SapBusinessPartner { get; set; }

        public string _1SapCentroCostoOcrCode { get; set; }
        public virtual SapCentroCosto _1SapCentroCosto { get; set; }

        public string _2SapCentroCostoOcrCode { get; set; }
        public virtual SapCentroCosto _2SapCentroCosto { get; set; }

        public string _3SapCentroCostoOcrCode { get; set; }
        public virtual SapCentroCosto _3SapCentroCosto { get; set; }

        public string _4SapCentroCostoOcrCode { get; set; }
        public virtual SapCentroCosto _4SapCentroCosto { get; set; }

        public string _5SapCentroCostoOcrCode { get; set; }
        public virtual SapCentroCosto _5SapCentroCosto { get; set; }

        public int TipoDocumentoSunatId { get; set; }
        public virtual TipoDocumentoSunat TipoDocumentoSunat { get; set; }

        public int SapMonedaId { get; set; }
        public virtual SapMoneda SapMoneda { get; set; }

        public int CreacionUsuarioId { get; set; }
        public virtual Usuario CreacionUsuario { get; set; }

        public int ModificacionUsuarioId { get; set; }
        public virtual  Usuario ModificacionUsuario { get; set; }

        #endregion

    }
}
