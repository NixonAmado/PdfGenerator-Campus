using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configurations;
public class AddressConfiguration : IEntityTypeConfiguration<Archivo>
{
    public void Configure(EntityTypeBuilder<Archivo> builder)
    {
        builder.ToTable("Archivo");
        builder.Property(key => key.Id)
        .IsRequired();
    
        builder.Property(p => p.Nombre)
        .IsRequired()
        .HasColumnType("varchar(50)");

        builder.Property(p => p.Extension)
        .IsRequired()
        .HasColumnType("varchar(5)");

        builder.Property(p => p.Tamanio)
        .IsRequired()
        .HasColumnType("float");

        builder.Property(p => p.Ubicacion)
        .IsRequired()
        .HasColumnType("text");
    }
}