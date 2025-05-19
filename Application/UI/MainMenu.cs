namespace CampusLove.Application.UI
{
    public class MainMenu
    {
        private readonly MenuRegistro _menuRegistro;
        private readonly MenuLogin _menuLogin;
        private readonly ViewEstadisticas _viewEstadisticas;
        public MainMenu(MenuRegistro menuRegistro, MenuLogin menuLogin, ViewEstadisticas viewEstadisticas)
        {
            _menuLogin = menuLogin;
            _menuRegistro = menuRegistro;
            _viewEstadisticas = viewEstadisticas;
        }
        public async Task MostrarMenu()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarEncabezado("💖 CAMPUS LOVE 💖");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n🌟 MENÚ PRINCIPAL 🌟");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. ✍️ Registrarse");
                Console.WriteLine("2. 🔑 Iniciar Sesión");
                Console.WriteLine("3. 📊 Mostrar estadísticas del sistema");
                Console.WriteLine("0. 🚪 Salir");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nSeleccione una opción: ");
                Console.ResetColor();
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        await _menuRegistro.MostrarMenuAsync();
                        break;
                    case "2":
                        await _menuLogin.MostrarLoginAsync();
                        break;
                    case "3":
                        await _viewEstadisticas.MostrarEstadisticasAsync();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        MostrarMensaje("⚠️ Opción no válida. Intente de nuevo.", ConsoleColor.DarkMagenta);
                        Console.ReadKey();
                        break;
                }
            }

            MostrarMensaje("\n💖 ¡Gracias por usar Campus Love! 💖", ConsoleColor.DarkGreen);
        }

        private void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{mensaje}");
            Console.ResetColor();
        }

        
        public static void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            string borde = new string('═', titulo.Length + 6);
            Console.WriteLine($"╔{borde}╗");
            Console.WriteLine($"║  {titulo}    ║");
            Console.WriteLine($"╚{borde}╝");

            Console.ResetColor();
        }
    }

    
}