using Dapper;
using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;

        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Actualizar(CuentaCreacionVM cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Cuentas
                                            SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                                            TipoCuentaId = @TipoCuentaId
                                            WHERE Id = @Id;", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Cuentas WHERE Id = @Id",
                new { id });
        }

        public async Task<IEnumerable<Cuentas>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuentas>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance,tc.Nombre as TipoCuenta
                                                            FROM Cuentas
                                                            INNER JOIN TiposCuentas tc
                                                            ON tc.Id = Cuentas.TipoCuentaId
                                                            WHERE tc.UsuarioId = @UsuarioId
                                                            ORDER BY TC.Orden",new {usuarioId});
        }

        public async Task Crear(Cuentas cuenta)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Cuentas(Nombre,TipoCuentaId,Descripcion,Balance)
                                                              VALUES (@Nombre,@TipoCuentaId,@Descripcion,@Balance);

                                                                SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id;
        }

        public async Task<Cuentas> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Cuentas>(
                                                            @"SELECT Cuentas.Id, Cuentas.Nombre, Balance,Descripcion,tc.Id
                                                            FROM Cuentas
                                                            INNER JOIN TiposCuentas tc
                                                            ON tc.Id = Cuentas.TipoCuentaId
                                                            WHERE tc.UsuarioId = @UsuarioId
                                                            AND Cuentas.Id = @Id", new {id, usuarioId });
        }


    }
}
