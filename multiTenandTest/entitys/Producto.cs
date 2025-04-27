using multiTenandTest.Helpers;

namespace multiTenandTest.entitys
{
    public class Producto : ITenandEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string TenantId { get; set; } = null!;
    }
}
