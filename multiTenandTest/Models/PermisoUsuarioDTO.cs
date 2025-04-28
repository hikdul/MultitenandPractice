using multiTenandTest.entitys;
using multiTenandTest.Helpers;

namespace multiTenandTest.Models
{
    public class PermisoUsuarioDTO
    {
        public Permisos Permiso { get; set; }
        public bool LoTiene { get; set; }
        public string? Descripcion { get; set; }
    }
}
