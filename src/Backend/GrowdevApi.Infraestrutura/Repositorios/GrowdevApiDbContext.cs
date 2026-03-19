using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Infraestrutura.ConfiguracaoDeDados;
using Microsoft.EntityFrameworkCore;

namespace GrowdevApi.Infraestrutura.Repositorios;

public class GrowdevApiDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ConfiguracaoUsuario());
        modelBuilder.ApplyConfiguration(new ConfiguracaoRefreshToken());
    }
}
