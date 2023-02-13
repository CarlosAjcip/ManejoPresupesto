using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interface
{
    public interface IRepositorioTransacciones
    {
        Task Crear(Transacciones transaccion);

        Task Actualizar(Transacciones transaccion, decimal montoAnterior, int cuentaAnteriorId);

        Task<Transacciones> ObtenerPorId(int id, int usuarioId);

        Task Borrar(int id);

        Task<IEnumerable<Transacciones>> ObtenerPorcuentaId(ObtenerTransaccionesPorCuenta modelo);
    }
}
