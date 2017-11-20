//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SICER.MODEL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documento()
        {
            this.Documento1 = new HashSet<Documento>();
            this.DocumentoEstadosAuditoria = new HashSet<DocumentoEstadosAuditoria>();
            this.Documento11 = new HashSet<Documento>();
        }
    
        public int DocumentoId { get; set; }
        public Nullable<int> AperturaDocumentoId { get; set; }
        public Nullable<int> EntregaRendirDocumentoId { get; set; }
        public int TipoDocumentoId { get; set; }
        public Nullable<int> SubTipoDocumento { get; set; }
        public int Estado { get; set; }
        public Nullable<int> NivelAprobacion { get; set; }
        public string SapIndicatorCode { get; set; }
        public string Codigo { get; set; }
        public string Asunto { get; set; }
        public string Motivo { get; set; }
        public string Serie { get; set; }
        public string Correlativo { get; set; }
        public System.DateTime FechaSolicitud { get; set; }
        public System.DateTime FechaDocumento { get; set; }
        public System.DateTime FechaContabilizacion { get; set; }
        public decimal MontoAfecto { get; set; }
        public decimal MontoNoAfecto { get; set; }
        public decimal MontoIgv { get; set; }
        public string SapMonedaDocCurrCode { get; set; }
        public string SapBusinessPartnerCardCode { get; set; }
        public string C_1SapCentroCostoOcrCode { get; set; }
        public string C_2SapCentroCostoOcrCode { get; set; }
        public string C_3SapCentroCostoOcrCode { get; set; }
        public string C_4SapCentroCostoOcrCode { get; set; }
        public string C_5SapCentroCostoOcrCode { get; set; }
        public string AsociadaSapCuentaContableCode { get; set; }
        public string GastoSapCuentaContableCode { get; set; }
        public string SapConceptoCode { get; set; }
        public string MotivoRechazo { get; set; }
        public string Error { get; set; }
        public System.DateTime CreacionFecha { get; set; }
        public int CreacionUsuarioid { get; set; }
        public Nullable<System.DateTime> ModificacionFecha { get; set; }
        public Nullable<int> ModificacionUsuarioid { get; set; }
        public string OstcCode { get; set; }
    
        public virtual SapCentroCosto SapCentroCosto { get; set; }
        public virtual SapCentroCosto SapCentroCosto1 { get; set; }
        public virtual SapCentroCosto SapCentroCosto2 { get; set; }
        public virtual SapCentroCosto SapCentroCosto3 { get; set; }
        public virtual SapCentroCosto SapCentroCosto4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento1 { get; set; }
        public virtual Documento Documento2 { get; set; }
        public virtual SapCuentaContable SapCuentaContable { get; set; }
        public virtual Usuario Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoEstadosAuditoria> DocumentoEstadosAuditoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento11 { get; set; }
        public virtual Documento Documento3 { get; set; }
        public virtual SapCuentaContable SapCuentaContable1 { get; set; }
        public virtual Usuario Usuario1 { get; set; }
        public virtual SapBusinessPartner SapBusinessPartner { get; set; }
        public virtual SapConcepto SapConcepto { get; set; }
        public virtual SapIndicators SapIndicators { get; set; }
        public virtual SapMoneda SapMoneda { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual OSTC OSTC { get; set; }
    }
}
