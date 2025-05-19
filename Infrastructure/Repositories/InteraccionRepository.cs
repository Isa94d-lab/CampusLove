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
            const string query = "INSERT INTO Interaccion (usuario_id, perfil_id, reaccion, fechaLike) VALUES (@UsuarioId, @PerfilId, 'Like', CURDATE())";

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
            const string query = "INSERT INTO Interaccion (usuario_id, perfil_id, reaccion, fechaLike) VALUES (@UsuarioId, @PerfilId, 'Dislike', CURDATE())";

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

        public async Task GuardarLikeAsync(int usuarioId, int perfilLikeadoId, bool like)
        {
            // Guarda el like normalmente en LikesUsuario
            const string query = @"
                INSERT INTO LikesUsuario (usuario_id, perfil_likeado_id, fechaLike, match_r)
                VALUES (@UsuarioId, @PerfilLikeadoId, @FechaLike, @MatchR)
                ON DUPLICATE KEY UPDATE
                    fechaLike = @FechaLike,
                    match_r = @MatchR;
            ";

            using (var command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                command.Parameters.AddWithValue("@PerfilLikeadoId", perfilLikeadoId);
                command.Parameters.AddWithValue("@FechaLike", DateTime.Now);
                command.Parameters.AddWithValue("@MatchR", like);

                await command.ExecuteNonQueryAsync();
            }

            // Si es un like (no dislike), verifica si hay like mutuo
            if (like)
            {
                // Busca si el perfil destino ya le dio like al usuario actual
                const string queryCheck = @"
                    SELECT 1 FROM LikesUsuario
                    WHERE usuario_id = (
                        SELECT usuario_id FROM Perfil WHERE id = @PerfilLikeadoId
                    )
                    AND perfil_likeado_id = (
                        SELECT id FROM Perfil WHERE id = (
                            SELECT perfil_id FROM Usuario WHERE id = @UsuarioId
                        )
                    )
                    AND match_r = 1
                    LIMIT 1;
                ";

                using (var command = new MySqlCommand(queryCheck, _connection))
                {
                    command.Parameters.AddWithValue("@PerfilLikeadoId", perfilLikeadoId);
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);

                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        // Hay match, registra en la tabla Matchs
                        await RegistrarMatchAsync(perfilLikeadoId, usuarioId);
                    }
                }
            }
        }
        public async Task RegistrarMatchAsync(int perfil1Id, int usuario2Id)
        {
            // Obtén el perfil_id del usuario2
            int perfil2Id = 0;
            const string queryPerfil = "SELECT perfil_id FROM Usuario WHERE id = @UsuarioId";
            using (var cmd = new MySqlCommand(queryPerfil, _connection))
            {
                cmd.Parameters.AddWithValue("@UsuarioId", usuario2Id);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null)
                    perfil2Id = Convert.ToInt32(result);
            }

            if (perfil2Id == 0) return;

            // Inserta el match (asegúrate de no duplicar)
            const string insertMatch = @"
                INSERT IGNORE INTO Matchs (perfil1_id, perfil2_id, fecha)
                VALUES (@Perfil1Id, @Perfil2Id, @Fecha)
            ";
            using (var cmd = new MySqlCommand(insertMatch, _connection))
            {
                cmd.Parameters.AddWithValue("@Perfil1Id", perfil1Id);
                cmd.Parameters.AddWithValue("@Perfil2Id", perfil2Id);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
