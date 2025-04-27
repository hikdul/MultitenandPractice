using System.ComponentModel.DataAnnotations;
using multiTenandTest.Helpers;

namespace multiTenandTest.Helpers
{
    public enum Permisos
    {
        [Esconder]
        nulo = 0, //?: este indica simplemen que un usuario pertenece a un grupo o empresa.

        [Display(Description = "Crear Productos")]
        prod_crear = 10,

        [Display(Description = "ver productos")]
        prod_ver = 11,
        user_vinc = 21,
        permisos_leer = 30,
        prermisos_editar = 31
    }
}
