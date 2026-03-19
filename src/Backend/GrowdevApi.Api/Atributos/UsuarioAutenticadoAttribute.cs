using GrowdevApi.Api.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace GrowdevApi.Api.Atributos;

public class UsuarioAutenticadoAttribute : TypeFilterAttribute
{
    public UsuarioAutenticadoAttribute() : base(typeof(FiltroUsuarioAutenticado))
    {
    }
}
