using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class PerfilInteresesRepository : IPerfilInteresesRepository
    {
        private readonly MySqlConnection _connection;

        public PerfilInteresesRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Intereses>> ObtenerInteresesDePerfilAsync(int perfilId)
        {
            var intereses = new List<Intereses>();
            const string query = @"
                SELECT i.id, i.descripcion
                FROM PerfilIntereses pi
                JOIN Intereses i ON pi.intereses_id = i.id
                WHERE pi.perfil_id = @PerfilId";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@PerfilId", perfilId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                intereses.Add(new Intereses
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString() ?? string.Empty
                });
            }

            return intereses;
        }

        public async Task<bool> AgregarInteresAsync(int perfilId, int interesId)
        {
            const string query = @"
                INSERT INTO PerfilIntereses (perfil_id, intereses_id)
                VALUES (@PerfilId, @InteresId)";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@PerfilId", perfilId);
                command.Parameters.AddWithValue("@InteresId", interesId);

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

        public async Task<bool> EliminarInteresAsync(int perfilId, int interesId)
        {
            const string query = @"
                DELETE FROM PerfilIntereses
                WHERE perfil_id = @PerfilId AND intereses_id = @InteresId";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@PerfilId", perfilId);
                command.Parameters.AddWithValue("@InteresId", interesId);

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
    }
}
