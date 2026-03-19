using GrowdevApi.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowdevApi.Infraestrutura.ConfiguracaoDeDados;

public class ConfiguracaoUsuario : ConfiguracaoEntidadeBase<Usuario>
{
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
        base.Configure(builder);

        builder.ToTable("Usuarios");

        builder.Property(usuario => usuario.Nome)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(usuario => usuario.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(usuario => usuario.Email)
           .IsUnique()
           .HasDatabaseName("UK_Usuarios_Email");

        builder.Property(usuario => usuario.Senha)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(usuario => usuario.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);
    }
}
