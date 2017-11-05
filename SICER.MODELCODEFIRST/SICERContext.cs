using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.MODELCODEFIRST.Tables;

namespace SICER.MODELCODEFIRST
{
    public class SicerContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Foreign Keys

            #region UsuarioTipoDocumento

            modelBuilder.Entity<UsuarioTipoDocumento>()
                   .HasRequired(m => m.Usuario)
                   .WithMany(t => t.UsuarioTipoDocumentos)
                   .HasForeignKey(m => m.UsuarioId)
                   .WillCascadeOnDelete(false);

            //modelBuilder.Entity<UsuarioTipoDocumento>()
            //    .HasRequired(m => m.TipoDocumento)
            //    .WithMany(t => t.UsuarioTipoDocumentos)
            //    .HasForeignKey(m => m.TipoDocumentoId)
            //    .WillCascadeOnDelete(false);

            #endregion

            #region Documento

            //modelBuilder.Entity<Documento>()
            //      .HasRequired(m => m.SAPBusinessPartner)
            //      .WithMany(t => t.Documentos)
            //      .HasForeignKey(m => m.SAPBusinessPartnerID)
            //      .WillCascadeOnDelete(false);

            modelBuilder.Entity<Documento>()
               .HasRequired(m => m.TipoDocumentoSunat)
               .WithMany(t => t.Documentos)
               .HasForeignKey(m => m.TipoDocumentoSunatId)
               .WillCascadeOnDelete(false);

            #endregion

            #region DocumentoRendicion

            modelBuilder.Entity<DocumentoRendicion>()
                 .HasRequired(m => m.CreacionUsuario)
                 .WithMany(t => t.CreacionDocumentoRendicions)
                 .HasForeignKey(m => m.CreacionUsuarioId)
                 .WillCascadeOnDelete(false);


            modelBuilder.Entity<DocumentoRendicion>()
                  .HasRequired(m => m.ModificacionUsuario)
                  .WithMany(t => t.ModificacionDocumentoRendicions)
                  .HasForeignKey(m => m.ModificacionUsuarioId)
                  .WillCascadeOnDelete(false);

            #endregion

      

            #endregion
        }

        public SicerContext() : base("SICER")
        {

        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<TipoDocumento> TipoDocumento { get; set; }
        public DbSet<AprobacionDocumento> AprobacionDocumento { get; set; }
        public DbSet<UsuarioTipoDocumento> UsuarioTipoDocumentoWeb { get; set; }
        public DbSet<Vista> Vista { get; set; }
        public DbSet<VistaRol> VistaRol { get; set; }
        public DbSet<GrupoVista> GrupoVista { get; set; }
        public DbSet<TipoDocumentoSunat> TipoDocumentoSunat { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<DocumentoRendicion> DocumentoRendicion { get; set; }
        //public DbSet<SapBusinessPartner> SAPBusinessPartner { get; set; }
        public DbSet<SapMetodoPago> SAPMetodoPago { get; set; }
        public DbSet<SapConcepto> SAPConcepto { get; set; }
    }
}
