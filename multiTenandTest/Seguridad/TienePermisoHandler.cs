using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using multiTenandTest.Data;
using multiTenandTest.Services;

namespace multiTenandTest.Seguridad
{
    public class TienePermisoHandler : AuthorizationHandler<TienePermisoRequirement>
    {
        private readonly IServicioTenant servicioTenant;
        private readonly IUsersService servicioUsuario;
        private readonly ApplicationDbContext dbContext;

        public TienePermisoHandler(
            IServicioTenant servicioTenant,
            IUsersService servicioUsuario,
            ApplicationDbContext dbContext
        )
        {
            this.servicioTenant = servicioTenant;
            this.servicioUsuario = servicioUsuario;
            this.dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TienePermisoRequirement requirement
        )
        {
            var permiso = requirement.Permiso;
            var usuarioId = servicioUsuario.getUserId();
            var tenantId = new Guid(servicioTenant.ObtenerTenant());

            var tienePermiso = await dbContext.EmpresaUsuarioPermisos.AnyAsync(x =>
                x.UserId == usuarioId && x.EmpresaId == tenantId && x.permiso == permiso
            );

            if (tienePermiso)
            {
                context.Succeed(requirement);
            }
        }
    }
}
