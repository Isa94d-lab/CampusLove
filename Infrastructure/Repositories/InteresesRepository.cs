using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class InteresesRepository : IInteresesRepository
    {
        private readonly MySqlConnection _connection;

        public InteresesRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Intereses>> ObtenerTodosAsync()
        {
            var intereses = new List<Intereses>();
            const string query = "SELECT id, tipo FROM Intereses";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                intereses.Add(new Intereses
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Tipo = reader["tipo"].ToString()
                });
            }

            return intereses;
        }

        public async Task<Intereses?> ObtenerPorIdAsync(int id)
        {
            const string query = "SELECT id, tipo FROM Intereses WHERE id = @Id";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Intereses
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Tipo = reader["tipo"].ToString()
                };
            }

            return null;
        }

        public async Task<bool> AgregarInteresAsync(string tipo)
        {
            const string query = "INSERT INTO Intereses (tipo) VALUES (@Tipo)";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Tipo", tipo);

            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }

        public async Task<bool> EliminarInteresAsync(int id)
        {
            const string query = "DELETE FROM Intereses WHERE id = @Id";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}
