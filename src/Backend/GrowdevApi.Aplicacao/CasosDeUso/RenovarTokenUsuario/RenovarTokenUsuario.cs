using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Entidades;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Tokens;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario;

public class RenovarTokenUsuario : IRenovarTokenUsuario
{
    private readonly IRepositorioRefreshToken _repositorioRefreshToken;
    private readonly IGeradorTokenUsuario _geradorTokenUsuario;
    private readonly IGeradorRefreshToken _geradorRefreshToken;
    private readonly IUnitOfWork _unitOfWork;

    public RenovarTokenUsuario(
        IRepositorioRefreshToken repositorioRefreshToken,
        IGeradorTokenUsuario geradorTokenUsuario,
        IGeradorRefreshToken geradorRefreshToken,
        IUnitOfWork unitOfWork)
    {
        _repositorioRefreshToken = repositorioRefreshToken;
        _geradorTokenUsuario = geradorTokenUsuario;
        _geradorRefreshToken = geradorRefreshToken;
        _unitOfWork = unitOfWork;
    }

    public async Task<RespostaTokens> Executar(RequisicaoNovoTokenUsuario requisicao, CancellationToken cancellationToken)
    {
        await Validar(requisicao, cancellationToken);

        var refreshToken = await _repositorioRefreshToken.PegarRefreshToken(requisicao.RefreshToken!, cancellationToken);

        if (refreshToken == null)
            throw new SessaoExpiradaExcecao();

        if (!_geradorRefreshToken.TokenValido(refreshToken))
            throw new SessaoExpiradaExcecao();

        return new RespostaTokens
        {
            AccessToken = _geradorTokenUsuario.Gerar(refreshToken.Usuario.Id),
            RefreshToken = await CriarESalvarRefreshToken(refreshToken.Usuario, cancellationToken)
        };
    }

    private static async Task Validar(RequisicaoNovoTokenUsuario requisicao, CancellationToken cancellationToken)
    {
        var validator = new RenovarTokenUsuarioValidador();

        var resultado = await validator.ValidateAsync(requisicao, cancellationToken);

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
            throw new ErroValidacaoExcecao(mensagensDeErro);
        }
    }

    private async Task<string> CriarESalvarRefreshToken(Dominio.Entidades.Usuario usuario, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            Valor = _geradorRefreshToken.Gerar(),
            IdUsuario = usuario.Id
        };

        await _repositorioRefreshToken.SalvarNovoRefreshToken(refreshToken, cancellationToken);
        await _unitOfWork.SalvarAsync(cancellationToken);

        return refreshToken.Valor;
    }
}
