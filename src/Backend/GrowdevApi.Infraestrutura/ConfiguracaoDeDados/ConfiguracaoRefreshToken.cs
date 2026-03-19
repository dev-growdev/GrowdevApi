using GrowdevApi.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowdevApi.Infraestrutura.ConfiguracaoDeDados;

public class ConfiguracaoRefreshToken : ConfiguracaoEntidadeBase<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("RefreshTokens");

        builder.Property(refreshToken => refreshToken.Valor)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.IdUsuario)
            .IsRequired();

        builder.HasAlternateKey(refreshToken => new { refreshToken.IdUsuario })
            .HasName("UK_RefreshTokens_IdUsuario");

        builder.HasOne(refreshToken => refreshToken.Usuario)
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.IdUsuario)
            .HasConstraintName("FK_RefreshTokens_Ref_Usuarios")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
