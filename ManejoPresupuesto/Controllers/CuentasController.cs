using AutoMapper;
using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuario servicioUsuario;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IMapper mapper;

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
            IServicioUsuario servicioUsuario, IRepositorioCuentas repositorioCuentas,
            IMapper mapper)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuario = servicioUsuario;
            this.repositorioCuentas = repositorioCuentas;
            this.mapper = mapper;
        }
        [HttpGet]
        public async  Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var modelo = new CuentaCreacionVM();
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(modelo);
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var cuentasTipoCuenta = await repositorioCuentas.Buscar(usuarioId);

            var modelo = cuentasTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndiceCuentasVM
                {
                    TipoCuenta = grupo.Key,
                    Cuenta = grupo.AsEnumerable()
                }).ToList();
            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionVM cuenta)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtnerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }

            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");

            }

            var modelo = mapper.Map<CuentaCreacionVM>(cuenta);

            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionVM cuentaEditar)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var tipoCuenta = await repositorioTiposCuentas.ObtnerPorId(cuentaEditar.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Actualizar(cuentaEditar);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.Borrar(id);
            return RedirectToAction("Index");
        }
    }
}
