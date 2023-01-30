using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServicioUsuario servicioUsuario;

        public CategoriasController(IRepositorioCategorias repositorioCategorias, IServicioUsuario servicioUsuario)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.servicioUsuario = servicioUsuario;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var categorias = await repositorioCategorias.Obtner(usuarioId);
            return View(categorias);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.UsuarioId = usuarioId;
            await repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPoId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaEditar)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditar);
            }
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPoId(categoriaEditar.Id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            categoriaEditar.UsuarioId = usuarioId;
            await repositorioCategorias.Actualizar(categoriaEditar);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var Existe = await repositorioCategorias.ObtenerPoId(id, usuarioId);

            if (Existe is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(Existe);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            var usuarioId = servicioUsuario.ObtnerUsuarioId();
            var Existe = await repositorioCategorias.ObtenerPoId(id, usuarioId);

            if (Existe is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCategorias.Borrar(id);
            return RedirectToAction("Index");
        }
    }
}
