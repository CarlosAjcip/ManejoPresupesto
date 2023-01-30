using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interface
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuario);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task Actualizar(TipoCuenta tipoCuenta);
        Task<TipoCuenta> ObtnerPorId(int id, int usuarioId);
        Task Borrar(int id);

         Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenar);
    }
}
