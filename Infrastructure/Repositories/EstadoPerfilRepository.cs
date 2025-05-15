using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class EstadoPerfilRepository : IGenericRepository<EstadoPerfil>, IEstadoPerfilRepository
    {
        private readonly MySqlConnection _connection;

        public EstadoPerfilRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<EstadoPerfil>> GetAllAsync()
        {
            var estados = new List<EstadoPerfil>();
            const string query = "SELECT id, descripcion FROM EstadoPerfil";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                estados.Add(new EstadoPerfil
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString() ?? string.Empty
                });
            }

            return estados;
        }

        public async Task<EstadoPerfil?> GetByIdAsync(object id)
        {
            const string query = "SELECT id, descripcion FROM EstadoPerfil WHERE id = @Id";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new EstadoPerfil
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString() ?? string.Empty
                };
            }

            return null;
        }

        public async Task<bool> InsertAsync(EstadoPerfil estado)
        {
            if (estado == null)
                throw new ArgumentNullException(nameof(estado));

            const string query = "INSERT INTO EstadoPerfil (descripcion) VALUES (@Descripcion)";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Descripcion", estado.Descripcion);

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

        public async Task<bool> UpdateAsync(EstadoPerfil estado)
        {
            if (estado == null)
                throw new ArgumentNullException(nameof(estado));

            const string query = "UPDATE EstadoPerfil SET descripcion = @Descripcion WHERE id = @Id";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Id", estado.Id);
                command.Parameters.AddWithValue("@Descripcion", estado.Descripcion);

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
            const string query = "DELETE FROM EstadoPerfil WHERE id = @Id";
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
