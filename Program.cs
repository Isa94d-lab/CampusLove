using CampusLove.Application.UI;
using CampusLove.Infrastructure.Configuration;
using CampusLove.Infrastructure.Repositories;
using MySql.Data.MySqlClient;

namespace CampusLove
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // Obtener la conexión a la base de datos
                var connection = DatabaseConfig.GetConnection();

                // Crear todos los repositorios requeridos por MenuRegistro
                var usuarioRepo = new UsuarioRepository(connection);
                var perfilRepo = new PerfilRepository(connection);
                var profesionRepo = new ProfesionRepository(connection);
                var generoRepo = new GeneroRepository(connection);
                var estadoPerfilRepo = new EstadoPerfilRepository(connection);

                // Crear el menú de registro con los repositorios
                var menuRegistro = new MenuRegistro(
                    usuarioRepo,
                    perfilRepo,
                    profesionRepo,
                    generoRepo,
                    estadoPerfilRepo
                );

                // Crear el menú de login con el repositorio de usuario
                var menuLogin = new MenuLogin(usuarioRepo);

                // Crear menú principal con menú de registro y login inyectados
                var mainMenu = new MainMenu(menuRegistro, menuLogin);

                // Mostrar el menú
                mainMenu.MostrarMenu();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                DatabaseConfig.CloseConnection();
            }
        }
    }
}
