﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SandBoxDB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SandBoxEntities : DbContext
    {
        public SandBoxEntities()
            : base("name=SandBoxEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Cat_DataTypes> Cat_DataTypes { get; set; }
        public DbSet<Cat_Tema> Cat_Tema { get; set; }
        public DbSet<Concepto> Concepto { get; set; }
        public DbSet<Cuestionarios> Cuestionarios { get; set; }
        public DbSet<Rel_ConceptoRespValor> Rel_ConceptoRespValor { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }
    }
}
