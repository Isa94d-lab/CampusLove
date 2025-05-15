using System;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class InteraccionRepository : IInteraccionRepository
    {
        private readonly MySqlConnection _connection;

        public InteraccionRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> DarLikeAsync(int usuarioId, int perfilId)
        {
            const string query = "INSERT INTO Interaccion (usuario_id, perfil_id, reaccion, fecha) VALUES (@UsuarioId, @PerfilId, 'Like', CURDATE())";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                command.Parameters.AddWithValue("@PerfilId", perfilId);

                var result = await command.ExecuteNonQueryAsync() > 0;
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DarDislikeAsync(int usuarioId, int perfilId)
        {
            const string query = "INSERT INTO Interaccion (usuario_id, perfil_id, reaccion, fecha) VALUES (@UsuarioId, @PerfilId, 'Dislike', CURDATE())";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                command.Parameters.AddWithValue("@PerfilId", perfilId);

                var result = await command.ExecuteNonQueryAsync() > 0;
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> YaInteraccionExisteAsync(int usuarioId, int perfilId)
        {
            const string query = "SELECT COUNT(*) FROM Interaccion WHERE usuario_id = @UsuarioId AND perfil_id = @PerfilId";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@UsuarioId", usuarioId);
            command.Parameters.AddWithValue("@PerfilId", perfilId);

            var result = Convert.ToInt32(await command.ExecuteScalarAsync());
            return result > 0;
        }
    }
}
