using multiTenandTest.entitys;

namespace multiTenandTest.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Producto> Productos { get; set; } = new List<Producto>();
        public IEnumerable<Pais> Países { get; set; } = new List<Pais>();
    }
}
