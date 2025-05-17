using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class ProfesionRepository : IGenericRepository<Profesion>, IProfesionRepository
    {
        private readonly MySqlConnection _connection;

        public ProfesionRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Profesion>> GetAllAsync()
        {
            var profesiones = new List<Profesion>();

            const string query = "SELECT id, descripcion FROM Profesion";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                profesiones.Add(new Profesion
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString()
                });
            }

            return profesiones;
        }

        public async Task<Profesion?> GetByIdAsync(object id)
        {
            const string query = "SELECT id, descripcion FROM Profesion WHERE id = @Id";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Profesion
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descripcion = reader["descripcion"].ToString()
                };
            }

            return null;
        }

        public async Task<int> InsertAsync(Profesion profesion)
        {
            if (profesion == null)
                throw new ArgumentNullException(nameof(profesion));

            const string query = "INSERT INTO Profesion (descripcion) VALUES (@Descripcion)";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Descripcion", profesion.Descripcion);

                await command.ExecuteNonQueryAsync();

                // Aquí se obtiene el ID insertado
                int profesionId = (int)command.LastInsertedId;

                await transaction.CommitAsync();
                return profesionId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Profesion profesion)
        {
            if (profesion == null) throw new ArgumentNullException(nameof(profesion));

            const string query = "UPDATE Profesion SET descripcion = @Descripcion WHERE id = @Id";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Id", profesion.Id);
                command.Parameters.AddWithValue("@Descripcion", profesion.Descripcion ?? string.Empty);

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
            const string query = "DELETE FROM Profesion WHERE id = @Id";

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
