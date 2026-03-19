using AutoMapper;
using GrowdevApi.Comunicacao.Requisicoes;
using GrowdevApi.Comunicacao.Respostas;
using GrowdevApi.Dominio.Entidades;

namespace GrowdevApi.Aplicacao.Servicos.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequisicaoParaDominio();
        DominioParaResposta();
    }

    private void RequisicaoParaDominio()
    {
        CreateMap<RequisicaoCadastrarUsuario, Usuario>();
        CreateMap<RequisicaoAlterarUsuario, Usuario>();
    }

    private void DominioParaResposta()
    {
        CreateMap<Usuario, RespostaDadosUsuario>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

    }
}
