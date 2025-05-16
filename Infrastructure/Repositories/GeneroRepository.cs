using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class GeneroRepository : IGenericRepository<Genero>, IGeneroRepository
    {
        private readonly MySqlConnection _connection;

        public GeneroRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Genero>> GetAllAsync()
        {
            var generos = new List<Genero>();
            const string query = "SELECT id, descripcion FROM Genero";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                generos.Add(new Genero
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString() ?? string.Empty
                });
            }

            return generos;
        }

        public async Task<Genero?> GetByIdAsync(object id)
        {
            const string query = "SELECT id, descripcion FROM Genero WHERE id = @Id";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Genero
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString() ?? string.Empty
                };
            }

            return null;
        }

        public async Task<bool> InsertAsync(Genero genero)
        {
            if (genero == null)
                throw new ArgumentNullException(nameof(genero));

            const string query = "INSERT INTO Genero (descripcion) VALUES (@Descripcion)";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Descripcion", genero.Descripcion);

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

        public async Task<bool> UpdateAsync(Genero genero)
        {
            if (genero == null)
                throw new ArgumentNullException(nameof(genero));

            const string query = "UPDATE Genero SET descripcion = @Descripcion WHERE id = @Id";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Id", genero.Id);
                command.Parameters.AddWithValue("@Descripcion", genero.Descripcion);

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

        public async Task<bool> DeleteAsync(object id)
        {
            const string query = "DELETE FROM Genero WHERE id = @Id";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Id", id);

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
