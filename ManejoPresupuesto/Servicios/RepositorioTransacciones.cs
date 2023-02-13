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
        private readonly string connectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Actualizar(Transacciones transaccion, decimal montoAnterior, int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("TransaccionesActualizar", new
            {
                transaccion.Id,
                transaccion.FechaTransacion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnteriorId
            },
            commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transacciones_Borrar",
                new { id },
                 commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Crear(Transacciones transaccion)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("Transaccion_Insertar", new
            {
                transaccion.UsuarioId,
                transaccion.FechaTransacion,
                transaccion.Monto,
                transaccion.Nota,
                transaccion.CuentaId,
                transaccion.CategoriaId,
                
                
            },
            commandType: System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
        }

        public async Task<IEnumerable<Transacciones>> ObtenerPorcuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Transacciones>
                (@"SELECT t.Id,t.Monto,t.FechaTransacion,c.Nombre as Categoria,cu.Nombre as Cuenta, c.TipoOperacionId
                    FROM Transacciones t
                    INNER JOIN Categorias c
                    ON c.Id = t.CategoriaId
                    INNER JOIN Cuentas cu
                    ON cu.Id = t.CuentaId
                    WHERE t.CuentaId = @CuentaId AND t.UsuarioId = @UsuarioId
                    AND FechaTransacion BETWEEN @FechaInicio AND @FechaFin", modelo);

        }

        public async Task<Transacciones> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Transacciones>(@"
            SELECT Transacciones.*, cat.TipoOperacionId
            FROM Transacciones
            INNER JOIN Categorias cat
            ON cat.Id = Transacciones.CategoriaId
            WHERE Transacciones.Id = @Id AND Transacciones.UsuarioId = @UsuarioId", new { id, usuarioId });

        }
    }
}
