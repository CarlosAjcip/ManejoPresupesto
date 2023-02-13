using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class CuentaCreacionVM : Cuentas
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
