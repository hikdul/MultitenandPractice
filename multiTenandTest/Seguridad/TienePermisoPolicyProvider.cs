﻿using Microsoft.AspNetCore.Authorization;
using multiTenandTest.Helpers;
using multiTenandTest.Services;

namespace multiTenandTest.Seguridad
{
    public class TienePermisoPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(
                new AuthorizationPolicyBuilder("Identity.Application")
                    .RequireAuthenticatedUser()
                    .Build()
            );
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy?>(null!);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string nombrePolitica)
        {
            if (
                nombrePolitica.StartsWith(
                    Constants.PrefijoPolitica,
                    StringComparison.OrdinalIgnoreCase
                )
                && Enum.TryParse(
                    typeof(Permisos),
                    nombrePolitica.Substring(Constants.PrefijoPolitica.Length),
                    out var permisoObj
                )
            )
            {
                var permiso = (Permisos)permisoObj!;
                var politica = new AuthorizationPolicyBuilder("Identity.Application");
                politica.AddRequirements(new TienePermisoRequirement(permiso));
                return Task.FromResult<AuthorizationPolicy?>(politica.Build());
            }

            return Task.FromResult<AuthorizationPolicy?>(null!);
        }
    }
}
