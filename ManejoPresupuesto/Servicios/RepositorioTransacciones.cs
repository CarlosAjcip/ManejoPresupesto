using Dapper;
using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Drawing;

namespace ManejoPresupuesto.Servicios
{
    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly string ConnectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Transaccion transaccion)
        {
            using var connection = new SqlConnection(ConnectionString);
            var id = await connection.QuerySingleAsync<int>("Transaccion_Insertar", new
            {
                transaccion.UsuarioId,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.Nota,
                transaccion.CuentaId,
                transaccion.CategoriaId,
                
                
            },
            commandType: System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
        }
    }
}
