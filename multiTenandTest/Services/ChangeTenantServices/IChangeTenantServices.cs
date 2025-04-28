
namespace multiTenandTest.Services
{
    public interface IChangeTenantServices
    {
        Task RepalceTenant(Guid idEmpresa, string usuarioId);
    }
}