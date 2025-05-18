using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class UsuarioRepository : IGenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly MySqlConnection _connection;

        public UsuarioRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();
            const string query = "SELECT id, perfil_id, nickname, password FROM Usuario";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                usuarios.Add(new Usuario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    PerfilId = Convert.ToInt32(reader["perfil_id"]),
                    Nickname = reader["nickname"].ToString(),
                    Password = reader["password"].ToString()
                });
            }

            return usuarios;
        }

        public async Task<Usuario?> GetByIdAsync(object id)
        {
            const string query = "SELECT id, perfil_id, nickname, password FROM Usuario WHERE id = @Id";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    PerfilId = Convert.ToInt32(reader["perfil_id"]),
                    Nickname = reader["nickname"].ToString(),
                    Password = reader["password"].ToString()
                };
            }

            return null;
        }

        public async Task<int> InsertAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            const string query = "INSERT INTO Usuario (perfil_id, nickname, password) VALUES (@PerfilId, @Nickname, @Password)";
            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@PerfilId", usuario.PerfilId);
                command.Parameters.AddWithValue("@Nickname", usuario.Nickname ?? string.Empty);
                command.Parameters.AddWithValue("@Password", usuario.Password ?? string.Empty);

                await command.ExecuteNonQueryAsync();
                int insertedId = (int)command.LastInsertedId;
                await transaction.CommitAsync();

                return insertedId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            const string query = "UPDATE Usuario SET perfil_id = @PerfilId, nickname = @Nickname, password = @Password WHERE id = @Id";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Id", usuario.Id);
                command.Parameters.AddWithValue("@PerfilId", usuario.PerfilId);
                command.Parameters.AddWithValue("@Nickname", usuario.Nickname ?? string.Empty);
                command.Parameters.AddWithValue("@Password", usuario.Password ?? string.Empty);

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
            const string query = "DELETE FROM Usuario WHERE id = @Id";

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

        public Usuario? ObtenerPorNickname(string nickname)
        {
            Usuario? usuario = null;
            const string query = "SELECT id, perfil_id, nickname, password FROM Usuario WHERE nickname = @nickname";
            using (var command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@nickname", nickname);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("id"),
                        Nickname = reader.GetString("nickname"),
                        Password = reader.GetString("password"),
                        PerfilId = reader.GetInt32("perfil_id")
                        // Asigna otras propiedades según tu modelo
                    };
                }
            }
            return usuario;
        }
    }
}
