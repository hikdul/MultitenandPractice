using Microsoft.AspNetCore.Identity;
using multiTenandTest.Helpers;

namespace multiTenandTest.entitys
{
    public class Vinculacion : ICommonEntity
    {
        public DateTime Creation { get; set; }
        public VinculationStatus Status { get; set; }
        public int Id { get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;
    }
}
