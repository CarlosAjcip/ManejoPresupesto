using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interface
{
    public interface IRepositorioTransacciones
    {
        Task Crear(Transaccion transaccion);
    }
}
