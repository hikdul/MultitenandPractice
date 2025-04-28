using System.ComponentModel.DataAnnotations;

namespace multiTenandTest.Models
{
    public class CrearEmpresa
    {
        [Required(ErrorMessage = "el cambpo {0} es requerido")]
        public string Nombre { get; set; }
    }
}
