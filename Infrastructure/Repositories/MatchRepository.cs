using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MySqlConnection _connection;

        public MatchRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Match>> ObtenerMatchesDeUsuarioAsync(int perfilId)
        {
            const string query = @"
                SELECT id, perfil1_id, perfil2_id, fecha
                FROM Match
                WHERE perfil1_id = @PerfilId OR perfil2_id = @PerfilId";

            var matches = new List<Match>();

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@PerfilId", perfilId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                matches.Add(new Match
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Perfil1Id = Convert.ToInt32(reader["perfil1_id"]),
                    Perfil2Id = Convert.ToInt32(reader["perfil2_id"]),
                    Fecha = Convert.ToDateTime(reader["fecha"])
                });
            }

            return matches;
        }

        public async Task CrearMatchAsync(int perfil1, int perfil2)
        {
            const string query = @"
                INSERT INTO Match (perfil1_id, perfil2_id, fecha)
                VALUES (@Perfil1Id, @Perfil2Id, CURDATE())";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Perfil1Id", perfil1);
                command.Parameters.AddWithValue("@Perfil2Id", perfil2);

                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
