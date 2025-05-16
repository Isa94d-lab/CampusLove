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
                // Configure database connection
                var connection = DatabaseConfig.GetConnection();

                // Connection
                var mainMenu = new MainMenu();
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