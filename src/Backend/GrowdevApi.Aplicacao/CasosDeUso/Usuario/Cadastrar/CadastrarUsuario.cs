using AutoMapper;
using FluentValidation.Results;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Interfaces.Criptografia;
using GrowdevApi.Dominio.Interfaces.Repositorios;
using GrowdevApi.Dominio.Interfaces.Servicos;
using GrowdevApi.Excecao;

namespace GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;

public class CadastrarUsuario : ICadastrarUsuario
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IUsuarioLogadoServico _usuarioLogadoServico;
    private readonly IEncriptadorSenha _encriptadorSenha;

    public CadastrarUsuario(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IRepositorioUsuario repositorioUsuario,
        IUsuarioLogadoServico usuarioLogadoServico,
        IEncriptadorSenha encriptadorSenha)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositorioUsuario = repositorioUsuario;
        _usuarioLogadoServico = usuarioLogadoServico;
        _encriptadorSenha = encriptadorSenha;
    }

    public async Task<RespostaDadosUsuario> Executar(RequisicaoCadastrarUsuario requisicao, CancellationToken cancellationToken)
    {
        await Validar(requisicao, cancellationToken);

        var usuario = _mapper.Map<Dominio.Entidades.Usuario>(requisicao);
        usuario.Status = Dominio.Enums.StatusUsuario.Ativo;
        usuario.Senha = _encriptadorSenha.Encriptar(requisicao.Senha!);
        await _repositorioUsuario.Adicionar(usuario, cancellationToken);
        await _unitOfWork.SalvarAsync(cancellationToken);
        return _mapper.Map<RespostaDadosUsuario>(usuario);
    }

    private async Task Validar(RequisicaoCadastrarUsuario requisicao, CancellationToken cancellationToken)
    {
        var validador = new CadastrarUsuarioValidador();

        var resultado = await validador.ValidateAsync(requisicao, cancellationToken);

        if (!string.IsNullOrWhiteSpace(requisicao.Email) && await _repositorioUsuario.ExisteUsuarioComEmail(requisicao.Email, cancellationToken))
            resultado.Errors.Add(new ValidationFailure(string.Empty, MensagensExcecao.EMAIL_JA_EXISTE));

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(e => e.ErrorMessage).Distinct().ToList();
            throw new ErroValidacaoExcecao(mensagensDeErro);
        }
    }
}
