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
    
    public partial class DocumentoEstadosAuditoria
    {
        public int DocumentoEstadosAuditoriaId { get; set; }
        public Nullable<int> DocumentoId { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public int Estado { get; set; }
        public Nullable<int> NumeroNivel { get; set; }
        public System.DateTime FechaAprobacion { get; set; }
    
        public virtual Documento Documento { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}