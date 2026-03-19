using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Enums;
using GrowdevApi.Dominio.Interfaces.Criptografia;
using GrowdevApi.Dominio.Interfaces.DadosIniciais;
using GrowdevApi.Infraestrutura.Repositorios;

namespace GrowdevApi.Infraestrutura.Inicializacao;

public class DadosIniciais : IDadosIniciais
{
    private readonly GrowdevApiDbContext _dbContext;
    private readonly IEncriptadorSenha _encriptadorSenha;
    public static Usuario UsuarioAdministrador = null!;

    public DadosIniciais(
        GrowdevApiDbContext dbContext,
        IEncriptadorSenha encriptadorSenha)
    {
        _dbContext = dbContext;
        _encriptadorSenha = encriptadorSenha;
    }

    public void Cadastrar()
    {
        CadastrarUsuariosIniciais();
    }

    private void CadastrarUsuariosIniciais()
    {
        if (!_dbContext.Usuarios.Any())
        {
            var senhaNaoEncriptada = "Senha.InicialGrowdevApiAdmin1";
            var senhaEncriptada = _encriptadorSenha.Encriptar(senhaNaoEncriptada);
            var usuarioAdministrador = new Usuario()
            {
                Nome = "Admin",
                Email = "admin@growdev.com",
                Senha = senhaEncriptada,
                Status = StatusUsuario.Ativo
            };
            _dbContext.Usuarios.Add(usuarioAdministrador);
            _dbContext.SaveChanges();
            UsuarioAdministrador = new Usuario()
            {
                Id = usuarioAdministrador.Id,
                Nome = usuarioAdministrador.Nome,
                Email = usuarioAdministrador.Email,
                Senha = senhaNaoEncriptada,
                Status = usuarioAdministrador.Status,
            };
        }
    }
}
