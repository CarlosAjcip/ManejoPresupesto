using Dapper;
using ManejoPresupuesto.Interface;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string conexion;
        public RepositorioCategorias(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("DefaultConnection");
        }

        
        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(conexion);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre,TipoOperacionId,UsuarioId)
                                                            Values (@Nombre,@TipoOperacionId,@UsuarioId);
                                                            SELECT SCOPE_IDENTITY();",categoria);

            categoria.Id = id;
        }

        public async Task<Categoria> ObtenerPoId(int id, int usuarioId)
        {
            using var connetion = new SqlConnection(conexion);

            return await connetion.QueryFirstAsync<Categoria>(@"SELECT * From Categorias 
                                                                WHERE Id = @Id AND UsuarioId = @UsuarioId",
                                                                new {id, usuarioId});
        }

        public async Task<IEnumerable<Categoria>> Obtner(int usuarioId)
        {
            using var connectio = new SqlConnection(conexion);

            return await connectio.QueryAsync<Categoria>("SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId"
                                                            , new {usuarioId});
        }
        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(conexion);
            await connection.ExecuteAsync("DELETE Categorias WHERE Id = @Id", new { id });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(conexion);
            await connection.ExecuteAsync(@"UPDATE Categorias
                                            SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                                            WHERE Id = @Id", categoria);
        }

        public async Task<IEnumerable<Categoria>> Obtner(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connectio = new SqlConnection(conexion);

            return await connectio.QueryAsync<Categoria>(@"SELECT * FROM
                                                            Categorias
                                                            WHERE UsuarioId = @UsuarioId AND TipoOperacionId = @tipoOperacionId"
                                                            , new { usuarioId, tipoOperacionId  });
        }
    }
}
