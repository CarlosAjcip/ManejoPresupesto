using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ManejoPresupuesto.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly IServicioUsuario servicioUsuario;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IRepositorioTransacciones repositorioTransacciones;

        public TransaccionesController(IServicioUsuario servicioUsuario, IRepositorioCuentas repositorioCuentas,
            IRepositorioCategorias repositorioCategorias, IRepositorioTransacciones repositorioTransacciones)
        {
            this.servicioUsuario = servicioUsuario;
            this.repositorioCuentas = repositorioCuentas;
            this.repositorioCategorias = repositorioCategorias;
            this.repositorioTransacciones = repositorioTransacciones;
        }
       

        // GET: TransaccionesController/Create
        public async Task<ActionResult> Create()
        {
             var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var modelo = new TransaccionVM();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoOperacionId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransaccionVM modelo)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.Cuentas = await ObtenerCuentas(usuarioId);
                modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoOperacionId);
                return View(modelo);
            }

            var cuenta = await repositorioCuentas.ObtenerPorId(modelo.CuentaId,usuarioId);
            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var categoria = await repositorioCategorias.ObtenerPoId(modelo.CategoriaId,usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            modelo.UsuarioId = usuarioId;
            if (modelo.TipoOperacionId == TipoOperacion.Gasto)
            {
                modelo.Monto *= -1;
            }

            await repositorioTransacciones.Crear(modelo);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            var cuentas = await repositorioCuentas.Buscar(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
        /// <summary>
        /// //
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="tipoOperacion"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, TipoOperacion tipoOperacion)
        {
            var categoria = await repositorioCategorias.Obtner(usuarioId, tipoOperacion);
            return categoria.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCategorias([FromBody] TipoOperacion tipoOperacion)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var categorias = await ObtenerCategorias(usuarioId, tipoOperacion);
            return Ok(categorias);
        }
    }
}
