
using System.ComponentModel.Design;

namespace CampusLove.Application.UI
{
    public class MainMenu
    {
        private readonly MenuRegistro _menuRegistro;
        private readonly MenuLogin _menuLogin;


        public void MostrarMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarEncabezado("CAMPUS LOVE");

                Console.WriteLine("\nMENÚ PRINCIPAL:");
                Console.WriteLine("1. Registrarse");
                Console.WriteLine("2. Iniciar Sesión");
                Console.WriteLine("0. Salir");

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        _menuRegistro.MostrarMenu();
                        break;
                    case "2":
                        _menuLogin.MostrarMenu();
                        break;

                    case "0":
                        salir = true;
                        break;
                    default:
                        MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        break;
                }
            }

            MostrarMensaje("\n¡Gracias por usar Campus Love!", ConsoleColor.DarkGreen);
        }

        private void MostrarMensaje(string v, ConsoleColor darkGreen)
        {
            throw new NotImplementedException();
        }

        public static void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            string borde = new string('=', titulo.Length + 4);
            Console.WriteLine(borde);
            Console.WriteLine($"| {titulo} |");
            Console.WriteLine(borde);

            Console.ResetColor();
        }
    }
    
}