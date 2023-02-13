using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [PrimerLetraMayus]
        [Required(ErrorMessage = "Ingrese un {0} Válido")]
        [Remote(action: "VerificarExisteTipoCuenta",controller:"TipoCuenta")]//seria la validacion del resultado desde el controlador del JSON
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}
