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
    
    public partial class SapBusinessPartner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SapBusinessPartner()
        {
            this.Usuario = new HashSet<Usuario>();
            this.Documento = new HashSet<Documento>();
        }
    
        public string SapBusinessPartnerCardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public string LictradNum { get; set; }
        public string validFor { get; set; }
        public string CardCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuario> Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }
    }
}
