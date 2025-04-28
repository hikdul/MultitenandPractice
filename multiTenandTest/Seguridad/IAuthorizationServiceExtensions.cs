using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using multiTenandTest.Helpers;
using multiTenandTest.Services;

namespace multiTenandTest.Seguridad
{
    public static class IAuthorizationServiceExtensions
    {
        public static async Task<bool> TienePermiso(
            this IAuthorizationService authorizationService,
            ClaimsPrincipal user,
            Permisos permiso
        )
        {
            if (!user.Identity!.IsAuthenticated)
                return false;

            var nombrePolitica = $"{Constants.PrefijoPolitica}{permiso}";
            var resultado = await authorizationService.AuthorizeAsync(user, nombrePolitica);
            return resultado.Succeeded;
        }
    }
}
