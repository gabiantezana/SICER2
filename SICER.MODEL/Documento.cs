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
            this.DocumentoDetalle = new HashSet<DocumentoDetalle>();
        }
    
        public int idDocumento { get; set; }
        public Nullable<int> idTipoOrigen { get; set; }
        public Nullable<int> idSAPProveedorSolicitante { get; set; }
        public Nullable<int> idCentroCostos1 { get; set; }
        public Nullable<int> idCentroCostos2 { get; set; }
        public Nullable<int> idCentroCostos3 { get; set; }
        public Nullable<int> idCentroCostos4 { get; set; }
        public Nullable<int> idCentroCostos5 { get; set; }
        public Nullable<int> idMetodoPago { get; set; }
        public Nullable<decimal> montoSolicitado { get; set; }
        public Nullable<decimal> montoGastado { get; set; }
        public Nullable<int> idMoneda { get; set; }
        public string asunto { get; set; }
        public string comentario { get; set; }
        public string motivoDetalle { get; set; }
        public Nullable<System.DateTime> fechaSolicitud { get; set; }
        public Nullable<int> estaAprobado { get; set; }
        public Nullable<int> estadoMigracion { get; set; }
        public Nullable<System.DateTime> fechaContabilizacion { get; set; }
        public Nullable<int> idUsuarioCreacion { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public Nullable<int> idUsuarioUltimoUpdate { get; set; }
        public Nullable<System.DateTime> fechaUltimoUpdate { get; set; }
    
        public virtual SAPCentroCostos SAPCentroCostos { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos1 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos2 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos3 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoDetalle> DocumentoDetalle { get; set; }
        public virtual SAPMetodoPago SAPMetodoPago { get; set; }
        public virtual SAPMoneda SAPMoneda { get; set; }
        public virtual SAPProveedor SAPProveedor { get; set; }
        public virtual TipoDocumentoOrigen TipoDocumentoOrigen { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Usuario1 { get; set; }
    }
}