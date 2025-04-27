using Microsoft.AspNetCore.Identity;
using multiTenandTest.Helpers;

namespace multiTenandTest.entitys
{
    public class EmpresaUsuarioPermisos: ICommonEntity
    {
        public string UserId { get; set; } = null!;
        public Guid EmpresaId { get; set; }
        public Permisos permiso { get; set; }
        public IdentityUser? User { get; set; }
        public Empresa? Empresa { get; set; }
    }
}
