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
    
    public partial class SapCuentaContable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SapCuentaContable()
        {
            this.Documento = new HashSet<Documento>();
            this.Documento1 = new HashSet<Documento>();
        }
    
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_MSS_DSCRPT { get; set; }
        public string U_MSS_IBACCT { get; set; }
        public string U_MSS_ACCT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento1 { get; set; }
    }
}
