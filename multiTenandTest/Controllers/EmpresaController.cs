using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using multiTenandTest.Data;
using multiTenandTest.entitys;
using multiTenandTest.Helpers;
using multiTenandTest.Models;
using multiTenandTest.Services;

public class EmpresaController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IUsersService usersServices;
    private readonly IChangeTenantServices changeTenantServices;

    public EmpresaController(
        ApplicationDbContext context,
        IUsersService usersServices,
        IChangeTenantServices changeTenantServices
    )
    {
        this.context = context;
        this.usersServices = usersServices;
        this.changeTenantServices = changeTenantServices;
    }

    #region crear empresa

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearEmpresa crearEmpresa)
    {
        if (!ModelState.IsValid)
            return View(crearEmpresa);
        Empresa nueva = new() { nombre = crearEmpresa.Nombre };

        var userId = usersServices.getUserId();
        nueva.userCreatedId = userId;
        context.Add(nueva);
        await context.SaveChangesAsync();

        List<EmpresaUsuarioPermisos> listUsuarioEmpresaPermiso = new();

        foreach (var p in Enum.GetValues<Permisos>())
        {
            listUsuarioEmpresaPermiso.Add(
                new()
                {
                    EmpresaId = nueva.Id,
                    UserId = userId,
                    permiso = p,
                }
            );
        }

        context.AddRange(listUsuarioEmpresaPermiso);
        await context.SaveChangesAsync();

        await changeTenantServices.RepalceTenant(nueva.Id, userId);

        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region  cambiar empresa

    [HttpGet]
    public async Task<IActionResult> Cambiar()
    {
        var userId = usersServices.getUserId();
        var list = await context
            .EmpresaUsuarioPermisos.Include(y => y.Empresa)
            .Where(y => y.UserId == userId)
            .Select(x => x.Empresa!)
            .Distinct()
            .ToListAsync();

        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> Cambiar(Guid id)
    {
        var userId = usersServices.getUserId();
        await changeTenantServices.RepalceTenant(id, userId);
        return RedirectToAction("Index", "Home");
    }

    #endregion
}
