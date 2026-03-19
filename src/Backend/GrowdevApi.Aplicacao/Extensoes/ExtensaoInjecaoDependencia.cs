using AutoMapper;
using GrowdevApi.Aplicacao.CasosDeUso.Limpeza.LimpezaLogs;
using GrowdevApi.Aplicacao.CasosDeUso.LoginUsuario;
using GrowdevApi.Aplicacao.CasosDeUso.RenovarTokenUsuario;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Alterar;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Cadastrar;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Excluir;
using GrowdevApi.Aplicacao.CasosDeUso.Usuario.Pegar;
using GrowdevApi.Aplicacao.Servicos.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace GrowdevApi.Aplicacao.Extensoes;


public static class ExtensaoInjecaoDependencia
{
    public static void AdicionarAplicacao(this IServiceCollection services)
    {
        AdicionarCasosDeUso(services);

        services.AddScoped(_ =>
            new MapperConfiguration(options => { options.AddProfile(new AutoMapping()); }).CreateMapper());
    }

    private static void AdicionarCasosDeUso(IServiceCollection services)
    {
        services.AddScoped<IFazerLoginUsuario, FazerLoginUsuario>();
        services.AddScoped<IRenovarTokenUsuario, RenovarTokenUsuario>();
        services.AddScoped<IPegarUsuarioLogado, PegarUsuarioLogado>();
        services.AddScoped<IPegarUsuarioPorId, PegarUsuarioPorId>();
        services.AddScoped<ICadastrarUsuario, CadastrarUsuario>();
        services.AddScoped<IAlterarUsuario, AlterarUsuario>();
        services.AddScoped<IExcluirUsuario, ExcluirUsuario>();
        services.AddScoped<ILimpezaArquivosLog, LimpezaArquivosLog>();
    }
}
