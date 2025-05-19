using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class EstadisticasRepository
    {
        private readonly MySqlConnection _connection;

        public EstadisticasRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<(string Nombre, string Apellido, int TotalLikes)?> ObtenerPerfilConMasLikesAsync()
        {
            const string query = @"
                SELECT p.nombre, p.apellido, COUNT(i.id) AS totalLikes
                FROM Perfil p
                JOIN Interaccion i ON p.id = i.perfil_id AND i.reaccion = 'Like'
                GROUP BY p.id
                ORDER BY totalLikes DESC
                LIMIT 1;";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return (
                    reader["nombre"].ToString() ?? "",
                    reader["apellido"].ToString() ?? "",
                    Convert.ToInt32(reader["totalLikes"])
                );
            }

            return null;
        }

        public async Task<(string Nombre, string Apellido, int TotalMatches)?> ObtenerPerfilConMasMatchesAsync()
        {
            const string query = @"
                SELECT p.nombre, p.apellido, COUNT(m.id) AS totalMatches
                FROM Perfil p
                JOIN Matchs m ON p.id = m.perfil1_id OR p.id = m.perfil2_id
                GROUP BY p.id
                ORDER BY totalMatches DESC
                LIMIT 1;";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return (
                    reader["nombre"].ToString() ?? "",
                    reader["apellido"].ToString() ?? "",
                    Convert.ToInt32(reader["totalMatches"])
                );
            }

            return null;
        }

        public async Task<(string Nickname, int TotalInteracciones)?> ObtenerUsuarioConMasInteraccionesAsync()
        {
            const string query = @"
                SELECT u.nickname, COUNT(i.id) AS totalInteracciones
                FROM Usuario u
                JOIN Perfil p ON u.perfil_id = p.id
                JOIN Interaccion i ON i.usuario_id = u.id
                GROUP BY u.id
                ORDER BY totalInteracciones DESC
                LIMIT 1;";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return (
                    reader["nickname"].ToString() ?? "",
                    Convert.ToInt32(reader["totalInteracciones"])
                );
            }

            return null;
        }

        public async Task<double> ObtenerPromedioEdadPerfilesAsync()
        {
            const string query = @"SELECT AVG(edad) AS promedioEdad FROM Perfil";

            using var command = new MySqlCommand(query, _connection);
            object? result = await command.ExecuteScalarAsync();

            return result != null && double.TryParse(result.ToString(), out double promedio)
                ? promedio
                : 0.0;
        }
    }
}
