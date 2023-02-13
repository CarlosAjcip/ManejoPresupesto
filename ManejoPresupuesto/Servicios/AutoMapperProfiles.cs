using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuentas, CuentaCreacionVM>();
            CreateMap<TransaccionActualizacionVM, Transacciones>().ReverseMap();
        }
    }
}
