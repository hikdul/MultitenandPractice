using Microsoft.AspNetCore.Identity;
using multiTenandTest.Helpers;

namespace multiTenandTest.Services
{
    public static class ExtentionType
    {
        public static bool DebeSaltarValidaciónTenant(this Type t)
        {
            var booleanos = new List<bool>()
            {
                t.IsAssignableFrom(typeof(IdentityRole)),
                t.IsAssignableFrom(typeof(IdentityRoleClaim<string>)),
                t.IsAssignableFrom(typeof(IdentityUser)),
                t.IsAssignableFrom(typeof(IdentityUserLogin<string>)),
                t.IsAssignableFrom(typeof(IdentityUserRole<string>)),
                t.IsAssignableFrom(typeof(IdentityUserToken<string>)),
                t.IsAssignableFrom(typeof(IdentityUserClaim<string>)),
                typeof(ICommonEntity).IsAssignableFrom(t)
            };

            var resultado = booleanos.Aggregate((b1, b2) => b1 || b2);

            return resultado;
        }
    }
}
