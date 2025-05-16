using System.ComponentModel.Design;

namespace CampusLove.Application.UI
{
    public class MainMenu
    {
        private readonly MenuRegistro _menuRegistro;
        // private readonly MenuLogin _menuLogin;

        public MainMenu(MenuRegistro menuRegistro)
        {
            _menuRegistro = menuRegistro;
        }

        public void MostrarMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarEncabezado("CAMPUS LOVE");

                Console.WriteLine("\nMENÚ PRINCIPAL:");
                Console.WriteLine("1. Registrarse (Usuario y Perfil)");
                Console.WriteLine("0. Salir");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nSeleccione una opción: ");
                Console.ResetColor();
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        _menuRegistro.MostrarMenu();
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

        private void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ResetColor();
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