using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Repositories
{
    public class PerfilRepository : IGenericRepository<Perfil>, IPerfilRepository
    {
        private readonly MySqlConnection _connection;

        public PerfilRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Perfil>> GetAllAsync()
        {
            var perfiles = new List<Perfil>();

            const string query = @"
                SELECT 
                    p.id, p.profesion_id, p.genero_id, p.estadoPerfil_id, p.nombre, p.apellido, p.edad, p.frase, p.gustos, p.coins,
                    pr.id AS pr_id, pr.descripcion AS pr_descripcion,
                    g.id AS g_id, g.descripcion AS g_descripcion,
                    e.id AS e_id, e.descripcion AS e_descripcion
                FROM Perfil p
                LEFT JOIN Profesion pr ON p.profesion_id = pr.id
                LEFT JOIN Genero g ON p.genero_id = g.id
                LEFT JOIN EstadoPerfil e ON p.estadoPerfil_id = e.id";

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                perfiles.Add(new Perfil
                {
                    Id = Convert.ToInt32(reader["id"]),
                    ProfesionId = Convert.ToInt32(reader["profesion_id"]),
                    GeneroId = Convert.ToInt32(reader["genero_id"]),
                    EstadoPerfilId = Convert.ToInt32(reader["estadoPerfil_id"]),
                    Nombre = reader["nombre"].ToString(),
                    Apellido = reader["apellido"].ToString(),
                    Edad = Convert.ToInt32(reader["edad"]),
                    Frase = reader["frase"].ToString(),
                    Gustos = reader["gustos"].ToString(),
                    Coins = Convert.ToInt32(reader["coins"]),
                    Profesion = new Profesion
                    {
                        Id = Convert.ToInt32(reader["pr_id"]),
                        Descripcion = reader["pr_descripcion"].ToString()
                    },
                    Genero = new Genero
                    {
                        Id = Convert.ToInt32(reader["g_id"]),
                        Descripcion = reader["g_descripcion"].ToString()
                    },
                    EstadoPerfil = new EstadoPerfil
                    {
                        Id = Convert.ToInt32(reader["e_id"]),
                        Descripcion = reader["e_descripcion"].ToString()
                    }
                });
            }

            return perfiles;
        }

        public async Task<Perfil?> GetByIdAsync(object id)
        {
            const string query = @"
                SELECT 
                    p.id, p.profesion_id, p.genero_id, p.estadoPerfil_id, p.nombre, p.apellido, p.edad, p.frase, p.gustos, p.coins,
                    pr.id AS pr_id, pr.descripcion AS pr_descripcion,
                    g.id AS g_id, g.descripcion AS g_descripcion,
                    e.id AS e_id, e.descripcion AS e_descripcion
                FROM Perfil p
                LEFT JOIN Profesion pr ON p.profesion_id = pr.id
                LEFT JOIN Genero g ON p.genero_id = g.id
                LEFT JOIN EstadoPerfil e ON p.estadoPerfil_id = e.id
                WHERE p.id = @Id";

            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Perfil
                {
                    Id = Convert.ToInt32(reader["id"]),
                    ProfesionId = Convert.ToInt32(reader["profesion_id"]),
                    GeneroId = Convert.ToInt32(reader["genero_id"]),
                    EstadoPerfilId = Convert.ToInt32(reader["estadoPerfil_id"]),
                    Nombre = reader["nombre"].ToString(),
                    Apellido = reader["apellido"].ToString(),
                    Edad = Convert.ToInt32(reader["edad"]),
                    Frase = reader["frase"].ToString(),
                    Gustos = reader["gustos"].ToString(),
                    Coins = Convert.ToInt32(reader["coins"]),
                    Profesion = new Profesion
                    {
                        Id = Convert.ToInt32(reader["pr_id"]),
                        Descripcion = reader["pr_descripcion"].ToString()
                    },
                    Genero = new Genero
                    {
                        Id = Convert.ToInt32(reader["g_id"]),
                        Descripcion = reader["g_descripcion"].ToString()
                    },
                    EstadoPerfil = new EstadoPerfil
                    {
                        Id = Convert.ToInt32(reader["e_id"]),
                        Descripcion = reader["e_descripcion"].ToString()
                    }
                };
            }

            return null;
        }

        public async Task<int> InsertAsync(Perfil perfil)
        {
            if (perfil == null) throw new ArgumentNullException(nameof(perfil));

            const string query = @"
                INSERT INTO Perfil (profesion_id, genero_id, estadoPerfil_id, nombre, apellido, edad, frase, gustos, coins)
                VALUES (@ProfesionId, @GeneroId, @EstadoPerfilId, @Nombre, @Apellido, @Edad, @Frase, @Gustos, @Coins)";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@ProfesionId", perfil.ProfesionId);
                command.Parameters.AddWithValue("@GeneroId", perfil.GeneroId);
                command.Parameters.AddWithValue("@EstadoPerfilId", perfil.EstadoPerfilId);
                command.Parameters.AddWithValue("@Nombre", perfil.Nombre ?? string.Empty);
                command.Parameters.AddWithValue("@Apellido", perfil.Apellido ?? string.Empty);
                command.Parameters.AddWithValue("@Edad", perfil.Edad);
                command.Parameters.AddWithValue("@Frase", perfil.Frase ?? string.Empty);
                command.Parameters.AddWithValue("@Gustos", perfil.Gustos ?? string.Empty);
                command.Parameters.AddWithValue("@Coins", perfil.Coins);

                await command.ExecuteNonQueryAsync();
                int perfilId = (int)command.LastInsertedId;

                await transaction.CommitAsync();
                return perfilId; 
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Perfil perfil)
        {
            if (perfil == null) throw new ArgumentNullException(nameof(perfil));

            const string query = @"
                UPDATE Perfil SET
                    profesion_id = @ProfesionId,
                    genero_id = @GeneroId,
                    estadoPerfil_id = @EstadoPerfilId,
                    nombre = @Nombre,
                    apellido = @Apellido,
                    edad = @Edad,
                    frase = @Frase,
                    gustos = @Gustos,
                    coins = @Coins
                WHERE id = @Id";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Id", perfil.Id);
                command.Parameters.AddWithValue("@ProfesionId", perfil.ProfesionId);
                command.Parameters.AddWithValue("@GeneroId", perfil.GeneroId);
                command.Parameters.AddWithValue("@EstadoPerfilId", perfil.EstadoPerfilId);
                command.Parameters.AddWithValue("@Nombre", perfil.Nombre ?? string.Empty);
                command.Parameters.AddWithValue("@Apellido", perfil.Apellido ?? string.Empty);
                command.Parameters.AddWithValue("@Edad", perfil.Edad);
                command.Parameters.AddWithValue("@Frase", perfil.Frase ?? string.Empty);
                command.Parameters.AddWithValue("@Gustos", perfil.Gustos ?? string.Empty);
                command.Parameters.AddWithValue("@Coins", perfil.Coins);

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
            const string query = "DELETE FROM Perfil WHERE id = @Id";
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

        public async Task<IEnumerable<Perfil>> GetPerfilesParaBusquedaAsync(Perfil perfilActual)
        {
            var perfiles = new List<Perfil>();

            // 1. Obtener los intereses del perfil actual
            var interesesActual = new List<int>();
            string interesesQuery = "SELECT intereses_id FROM PerfilIntereses WHERE perfil_id = @PerfilId";
            using (var interesesCmd = new MySqlCommand(interesesQuery, _connection))
            {
                interesesCmd.Parameters.AddWithValue("@PerfilId", perfilActual.Id);
                using (var reader = await interesesCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        interesesActual.Add(Convert.ToInt32(reader["InteresesId"]));
                    }
                }
            }

            if (interesesActual.Count == 0)
                return perfiles; // No hay intereses, no hay sugerencias

            // 2. Buscar perfiles compatibles (que tengan al menos un interés en común y no sean el propio)
            string interesesIds = string.Join(",", interesesActual);
            string query = $@"
                SELECT DISTINCT p.*
                FROM Perfil p
                INNER JOIN PerfilIntereses pi ON p.Id = pi.perfil_id
                WHERE p.Id <> @IdActual
                AND pi.InteresesId IN ({interesesIds})
                -- Filtro de género si lo necesitas, por ejemplo:
                -- AND (p.Genero = 'masculino' OR p.Genero = 'femenino')
            ";

            using (var command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdActual", perfilActual.Id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        perfiles.Add(new Perfil
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            // Asigna aquí los demás campos de Perfil
                        });
                    }
                }
            }
            return perfiles;
        }

        internal void Guardar(Perfil perfil)
        {
            throw new NotImplementedException();
        }
    }
}
