using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interface
{
    public interface IRepositorioCategorias
    {
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtner(int usuarioId);
        Task<IEnumerable<Categoria>> Obtner(int usuarioId,TipoOperacion tipoOperacionId);

        Task<Categoria> ObtenerPoId(int id, int usuarioId);
        Task Borrar(int id);

        Task Actualizar(Categoria categoria);
    }
}
