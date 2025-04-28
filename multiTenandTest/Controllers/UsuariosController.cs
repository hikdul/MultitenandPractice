using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using multiTenandTest.Models;
using multiTenandTest.Data;
using multiTenandTest.Helpers;
using multiTenandTest.Services;

namespace multiTenandTest.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IChangeTenantServices servicioCambioTenant;

        public UsuariosController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IChangeTenantServices servicioCambioTenant
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.servicioCambioTenant = servicioCambioTenant;
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(
                modelo.Email,
                modelo.Password,
                modelo.Recuerdame,
                lockoutOnFailure: false
            );

            if (resultado.Succeeded)
            {
                var usuario = await userManager.FindByEmailAsync(modelo.Email);
                var empresasVinculadas = await context
                    .EmpresaUsuarioPermisos.Where(x =>
                        x.UserId == usuario.Id && x.permiso == Permisos.nulo
                    )
                    .OrderBy(x => x.EmpresaId)
                    .Take(2)
                    .Select(x => x.EmpresaId)
                    .ToListAsync();

                if (empresasVinculadas.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (empresasVinculadas.Count == 1)
                {
                    await servicioCambioTenant.RepalceTenant(empresasVinculadas[0], usuario.Id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Cambiar", "Empresas");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(modelo);
            }
        }
    }
}
