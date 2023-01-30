using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interface
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuentas cuenta);
        Task<IEnumerable<Cuentas>> Buscar(int usuarioId);

        Task<Cuentas> ObtenerPorId(int id, int usuarioId);
        Task Actualizar(CuentaCreacionVM cuenta);

        Task Borrar(int id);
    }
}
