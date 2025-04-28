using Microsoft.AspNetCore.Authorization;
using multiTenandTest.Helpers;

namespace multiTenandTest.Seguridad
{
    public class TienePermisoRequirement: IAuthorizationRequirement
    {
        public TienePermisoRequirement(Permisos permisos)
        {
            Permiso = permisos;
        }

        public Permisos Permiso { get; }
    }
}
