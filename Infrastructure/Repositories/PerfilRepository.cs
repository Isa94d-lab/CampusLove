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

            // 1. Obtener el interés del perfil actual
            int interesId = 0;
            string interesQuery = "SELECT intereses_id FROM PerfilIntereses WHERE perfil_id = @PerfilId LIMIT 1";
            using (var interesCmd = new MySqlCommand(interesQuery, _connection))
            {
                interesCmd.Parameters.AddWithValue("@PerfilId", perfilActual.Id);
                using (var reader = await interesCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                        interesId = Convert.ToInt32(reader["intereses_id"]);
                }
            }

            if (interesId == 0)
                return perfiles; // No hay interés definido

            // 2. Determinar filtro de genero_id según el interés
            string filtroGenero;
            if (interesId == 1) // hombres
                filtroGenero = "p.genero_id = 1";
            else if (interesId == 2) // mujeres
                filtroGenero = "p.genero_id = 2";
            else // ambos
                filtroGenero = "(p.genero_id = 1 OR p.genero_id = 2)";

            // 3. Buscar perfiles compatibles, excluyendo el propio
            string query = $@"
                SELECT p.*, g.descripcion AS genero
                FROM Perfil p
                JOIN Genero g ON p.genero_id = g.id
                WHERE p.id <> @IdActual
                AND {filtroGenero}
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
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),
                            Edad = Convert.ToInt32(reader["edad"]),
                            Frase = reader["frase"].ToString(),
                            Gustos = reader["gustos"].ToString(),
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

        // Funcion encargada de actualizar los coins cada vez que se le de like al respectivo usuario/perfil
        public async Task ActualizarCoinsAsync(Perfil perfil)
        {
            const string query = "UPDATE Perfil SET coins = @Coins WHERE id = @Id";

            using var transaction = await _connection.BeginTransactionAsync();

            try
            {
                using var command = new MySqlCommand(query, _connection, transaction);
                command.Parameters.AddWithValue("@Coins", perfil.Coins);
                command.Parameters.AddWithValue("@Id", perfil.Id);

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
