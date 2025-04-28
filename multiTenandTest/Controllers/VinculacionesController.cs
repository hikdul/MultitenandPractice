using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using multiTenandTest.Data;
using multiTenandTest.entitys;
using multiTenandTest.Helpers;
using multiTenandTest.Models;
using multiTenandTest.Services;

namespace multiTenandTest.Controllers
{
    [Authorize]
    public class VinculacionesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService servicioUsuario;
        private readonly IServicioTenant servicioTenant;

        public VinculacionesController(ApplicationDbContext context,
            IUsersService servicioUsuario, IServicioTenant servicioTenant)
        {
            this.context = context;
            this.servicioUsuario = servicioUsuario;
            this.servicioTenant = servicioTenant;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuario.getUserId();
            return await RetornarVinculacionesPendientes(usuarioId);
        }

        private async Task<IActionResult> RetornarVinculacionesPendientes(string usuarioId)
        {
            var vinculacionesPendientes = await context.Vinculacion
                .Include(x => x.Empresa)
                .Where(x => x.Status == VinculationStatus.pendiente
                && x.UserId == usuarioId).ToListAsync();
            
            return View(vinculacionesPendientes);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid empresaId,
            VinculationStatus vinculacionEstatus)
        {
            var usuarioId = servicioUsuario.getUserId();
            var vinculacion = await context.Vinculacion
                    .FirstOrDefaultAsync(x => x.UserId == usuarioId
                    && x.EmpresaId == empresaId
                    && x.Status == VinculationStatus.pendiente);

            if (vinculacion is null)
            {
                ModelState.AddModelError("", "Ha habido un error: vinculación no encontrada");
                return await RetornarVinculacionesPendientes(usuarioId);
            }

            if (vinculacionEstatus == VinculationStatus.aceptada)
            {
                var permisoNulo = new EmpresaUsuarioPermisos()
                {
                    permiso = Permisos.nulo,
                    EmpresaId = empresaId,
                    UserId = usuarioId
                };

                context.Add(permisoNulo);
            }

            vinculacion.Status = vinculacionEstatus;
            await context.SaveChangesAsync();

            return RedirectToAction("Cambiar", "Empresas");
        }

        public async Task<IActionResult> Vincular()
        {
            var empresaId = servicioTenant.ObtenerTenant();

            if (string.IsNullOrEmpty(empresaId))
            {
                return RedirectToAction("Index", "Home");
            }

            var empresaIdGuid = new Guid(empresaId);

            var empresa = await context
                .Empresa.FirstOrDefaultAsync(x => x.Id == empresaIdGuid);

            if (empresa is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var modelo = new VincularUsuario
            {
                EmpresaId = empresa.Id,
                NombreEmpresa = empresa.nombre
            };

            return View(modelo);


        }

        [HttpPost]
        public async Task<IActionResult> Vincular(VincularUsuario modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuarioAVincular = await context.Users
                .FirstOrDefaultAsync(x => x.Email == modelo.EmailUsuario);

            if (usuarioAVincular is null)
            {
                ModelState.AddModelError(nameof(modelo.EmailUsuario), "No existe un usuario con ese email");
                return View(modelo);
            }

            var vinculacion = new Vinculacion
            {
                EmpresaId = modelo.EmpresaId,
                UserId = usuarioAVincular.Id,
                Status = VinculationStatus.pendiente,
                Creation = DateTime.UtcNow
            };

            context.Add(vinculacion);
            await context.SaveChangesAsync();
            return RedirectToAction("UsuarioVinculado");
        }

        public IActionResult UsuarioVinculado()
        {
            return View();
        }

    }
}
