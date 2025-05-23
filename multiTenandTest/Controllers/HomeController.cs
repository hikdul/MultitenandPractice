using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using multiTenandTest.Data;
using multiTenandTest.entitys;
using multiTenandTest.Models;
using multiTenandTest.Seguridad;

namespace multiTenandTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = await ConstruirModeloHomeIndex();
            return View(modelo);
        }

        [HttpPost]
        [TienePermiso(Helpers.Permisos.prod_crear)]
        public async Task<IActionResult> Index(Producto producto)
        {
            context.Add(producto);
            await context.SaveChangesAsync();
            var modelo = await ConstruirModeloHomeIndex();
            return View(modelo);
        }

        private async Task<HomeIndexViewModel> ConstruirModeloHomeIndex()
        {
            var productos = await context.Productos.ToListAsync();
            var paises = await context.Paises.ToListAsync();

            var modelo = new HomeIndexViewModel();
            modelo.Productos = productos;
            modelo.Países = paises;
            return modelo;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                }
            );
        }
    }
}
