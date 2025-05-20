using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class LikesDiariosRepository : ILikesDiariosRepository
    {
        private readonly MySqlConnection _connection;

        public LikesDiariosRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<LikesDiarios> ObtenerRegistroDePerfilAsync(int perfilId)
        {
            const string query = "SELECT id, perfil_id, cantidad FROM LikesDiarios WHERE perfil_id = @PerfilId";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@PerfilId", perfilId);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new LikesDiarios
                {
                    Id = Convert.ToInt32(reader["id"]),
                    PerfilId = Convert.ToInt32(reader["perfil_id"]),
                    Cantidad = Convert.ToInt32(reader["cantidad"])
                };
            }

            return new LikesDiarios { PerfilId = perfilId, Cantidad = 0 };
        }

        public async Task<int> ObtenerLikesDisponiblesAsync(int perfilId)
        {
            const string query = "SELECT cantidad FROM LikesDiarios WHERE perfil_id = @PerfilId";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@PerfilId", perfilId);

            var result = await command.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> RestarLikeAsync(int perfilId)
        {
            const string query = "UPDATE LikesDiarios SET cantidad = cantidad - 1 WHERE perfil_id = @PerfilId AND cantidad > 0";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
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

        public async Task<bool> EstablecerLikesInicialesAsync(int perfilId, int cantidad)
        {
            const string query = @"
                INSERT INTO LikesDiarios (perfil_id, cantidad)
                VALUES (@PerfilId, @Cantidad)
                ON DUPLICATE KEY UPDATE cantidad = @Cantidad";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@PerfilId", perfilId);
                command.Parameters.AddWithValue("@Cantidad", cantidad);

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

        public async Task<bool> ReestablecerLikesSiEsNuevoDiaAsync(int perfilId)
        {
            // Aquí deberías tener una columna de fecha para saber si ya es otro día
            // Supongamos que agregaste una columna 'fecha_actualizacion' DATE en la tabla LikesDiarios
            const string selectQuery = "SELECT cantidad, DATE(fecha_actualizacion) FROM LikesDiarios WHERE perfil_id = @PerfilId";
            const string updateQuery = "UPDATE LikesDiarios SET cantidad = 10, fecha_actualizacion = CURDATE() WHERE perfil_id = @PerfilId";

            using var command = new MySqlCommand(selectQuery, _connection);
            command.Parameters.AddWithValue("@PerfilId", perfilId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var fechaUltima = Convert.ToDateTime(reader["fecha_actualizacion"]).Date;
                if (fechaUltima < DateTime.Now.Date)
                {
                    reader.Close();
                    using var updateCmd = new MySqlCommand(updateQuery, _connection);
                    updateCmd.Parameters.AddWithValue("@PerfilId", perfilId);
                    return await updateCmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return false;
        }
    }
}
