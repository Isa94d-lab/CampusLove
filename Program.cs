using CampusLove.Application.UI;
using CampusLove.Infrastructure.Configuration;
using CampusLove.Infrastructure.Repositories;
using MySql.Data.MySqlClient;

namespace CampusLove
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                var connection = DatabaseConfig.GetConnection();

                var usuarioRepo = new UsuarioRepository(connection);
                var perfilRepo = new PerfilRepository(connection);
                var profesionRepo = new ProfesionRepository(connection);
                var generoRepo = new GeneroRepository(connection);
                var estadoPerfilRepo = new EstadoPerfilRepository(connection);

                // Aquí creas los repositorios que te faltaban
                var interesesRepo = new InteresesRepository(connection);
                var perfilInteresesRepo = new PerfilInteresesRepository(connection);

                var menuRegistro = new MenuRegistro(
                    usuarioRepo,
                    perfilRepo,
                    profesionRepo,
                    generoRepo,
                    estadoPerfilRepo,
                    interesesRepo,           
                    perfilInteresesRepo      
                );

                var interaccionRepo = new InteraccionRepository(connection);

                var menuLogin = new MenuLogin(usuarioRepo, perfilRepo, interaccionRepo);

                var mainMenu = new MainMenu(menuRegistro, menuLogin);

                await mainMenu.MostrarMenu();
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
