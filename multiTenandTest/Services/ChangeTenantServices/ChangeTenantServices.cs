using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using multiTenandTest.Data;

namespace multiTenandTest.Services
{
    public class ChangeTenantServices : IChangeTenantServices
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;

        public ChangeTenantServices(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task RepalceTenant(Guid idEmpresa, string usuarioId)
        {
            var user = await userManager.FindByIdAsync(usuarioId);

            //?: verificamos si existe un claim para el usuario actual
            var ExitsClaimTenant = await context.UserClaims.FirstOrDefaultAsync(x =>
                x.ClaimType == Constants.ClaimTenantId && x.UserId == usuarioId
            );
            //?: si es asi lo eliminamos
            if (ExitsClaimTenant is not null)
                context.Remove(ExitsClaimTenant);
            //?: creamos el claim
            var newClaim = new Claim(Constants.ClaimTenantId, idEmpresa.ToString());
            //?: lo agregamos
            await userManager.AddClaimAsync(user, newClaim);
            //?: aplicamos los cambios
            await signInManager.SignInAsync(user, isPersistent: true);
        }
    }
}
