using GrowdevApi.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowdevApi.Infraestrutura.ConfiguracaoDeDados;

public class ConfiguracaoEntidadeBase<T> : IEntityTypeConfiguration<T> where T : EntidadeBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        var tableName = builder.Metadata.GetTableName();

        builder.HasKey(entidade => entidade.Id)
            .HasName($"PK_{tableName}");

        builder.Property(entidade => entidade.Id)
            .IsRequired();

        builder.Property(entidade => entidade.DataCriacaoUtc)
            .IsRequired();
    }
}
