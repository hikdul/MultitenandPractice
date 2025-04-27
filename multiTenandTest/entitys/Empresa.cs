
using Microsoft.AspNetCore.Identity;
using multiTenandTest.Helpers;

namespace multiTenandTest.entitys
{
    public class Empresa: ICommonEntity
    {
        public Guid Id { get; set; }
        public string nombre { get; set; } = null!;
        public string? userCreated { get; set; } 
        public IdentityUser userCreatedId { get; set; } = null!;
        public List<EmpresaUsuarioPermisos> EmpresaUsuarioPermisos { get; set; } = null!;
    }
}