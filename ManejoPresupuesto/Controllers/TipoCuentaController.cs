using Dapper;
using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TipoCuentaController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuario servicioUsuario;

        public TipoCuentaController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuario servicioUsuario)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuario = servicioUsuario;
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta) 
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = servicioUsuario.ObtnerUsuarioId();//se obtiene el id del usuario que devuelve 1 
            //pasando los parametros para poder validar si ya existe o no
            var yaExiste = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExiste)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }
            await repositorioTiposCuentas.Crear(tipoCuenta);

            return RedirectToAction("Index");
        }

        //actualizar
        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var usarioId = servicioUsuario.ObtnerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtnerPorId(id, usarioId);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var tipoCuentaExis = await repositorioTiposCuentas.ObtnerPorId(tipoCuenta.Id, usuarioId);

            if (tipoCuentaExis is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]//toma el parametro desde el usuario y hace la validacion desde que deja la caja del input o se camia de caja y devuelve un valor
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);

            if (yaExisteTipoCuenta)
            {
                return Json($"el nombre {nombre} ya existe");
            }
            return Json(true);
        }

        //listado tipos cuentas
        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var tipos = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tipos);
        }

        //Borrar
        public async Task<IActionResult> borrar(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtnerPorId(id,usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrartipoCuenta(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtnerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usarioId = servicioUsuario.ObtnerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id);

            var tiposCuentasNo = ids.Except(idsTiposCuentas).ToList();

            if (tiposCuentasNo.Count > 0)
            {
                return Forbid();
            }
            var tiposCuentasOrdenados = ids.Select((valor, indice) =>
            new TipoCuenta() { Id = valor, Orden = indice + 1 }).AsEnumerable();

            await repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);

            return Ok();
        }
    }
}
