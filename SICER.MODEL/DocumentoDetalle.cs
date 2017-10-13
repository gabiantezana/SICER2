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
    
    public partial class DocumentoDetalle
    {
        public int idDocumentoDetalle { get; set; }
        public Nullable<int> idDocumento { get; set; }
        public Nullable<int> idProveedor { get; set; }
        public Nullable<int> idConcepto { get; set; }
        public Nullable<int> idTipoDocumentoOrigen { get; set; }
        public string serieDoc { get; set; }
        public string correlativoDoc { get; set; }
        public Nullable<System.DateTime> fechaDocumento { get; set; }
        public Nullable<int> idMonedaDoc { get; set; }
        public Nullable<int> idCentroCosto1 { get; set; }
        public Nullable<int> idCentroCosto2 { get; set; }
        public Nullable<int> idCentroCosto3 { get; set; }
        public Nullable<int> idCentroCosto4 { get; set; }
        public Nullable<int> idCentroCosto5 { get; set; }
        public Nullable<int> idCentroCosto6 { get; set; }
        public string montoDoc { get; set; }
        public string tasaCambio { get; set; }
        public Nullable<int> idMonedaOriginal { get; set; }
        public Nullable<decimal> montoNoAfecto { get; set; }
        public Nullable<decimal> montoAfecto { get; set; }
        public Nullable<decimal> montoIGV { get; set; }
        public Nullable<decimal> montoTotal { get; set; }
        public Nullable<int> idUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> fechaCreacion { get; set; }
        public Nullable<int> idUsuarioUltimoUpdate { get; set; }
        public Nullable<System.DateTime> fechaUltimoUpdate { get; set; }
    
        public virtual Concepto Concepto { get; set; }
        public virtual Documento Documento { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos1 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos2 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos3 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos4 { get; set; }
        public virtual SAPCentroCostos SAPCentroCostos5 { get; set; }
        public virtual SAPMoneda SAPMoneda { get; set; }
        public virtual SAPMoneda SAPMoneda1 { get; set; }
        public virtual SAPProveedor SAPProveedor { get; set; }
        public virtual TipoDocumentoOrigen TipoDocumentoOrigen { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Usuario1 { get; set; }
    }
}