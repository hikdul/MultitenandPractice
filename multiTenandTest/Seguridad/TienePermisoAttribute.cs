using Microsoft.AspNetCore.Authorization;
using multiTenandTest.Helpers;
using multiTenandTest.Services;

namespace multiTenandTest.Seguridad
{
    public class TienePermisoAttribute : AuthorizeAttribute
    {
        public TienePermisoAttribute(Permisos permisos)
        {
            Permisos = permisos;
        }

        public Permisos Permisos
        {
            get
            {
                // TienePermisoProductos_Crear
                if (
                    Enum.TryParse(
                        typeof(Permisos),
                        Policy!.Substring(Constants.PrefijoPolitica.Length),
                        ignoreCase: true,
                        out var permiso
                    )
                )
                {
                    return (Permisos)permiso!;
                }

                return Permisos.nulo;
            }
            set { Policy = $"{Constants.PrefijoPolitica}{value.ToString()}"; }
        }
    }
}
