using System;
using CampusLove.Application.UI;
using CampusLove.Infrastructure.Repositories;


namespace CampusLove.Application.UI
{
    public class MenuLogin
    {
        private readonly UsuarioRepository _usuarioRepository;

        public MenuLogin(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        public bool MostrarLogin()
        {
            Console.Clear();
            MostrarEncabezado(" LOGIN ");
            Console.Write("Nickname: ");
            string nickname = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            string password = LeerPassword();

            bool esValido = VerificarCredenciales(nickname, password);

            if (esValido)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n¡Login exitoso!");

                // Conexion si Login es existoso, con el menu del usuario
                var menuUsuario = new MenuUsuario(nickname);
                menuUsuario.MostrarMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNickname o contraseña incorrectos.");
            }
            Console.ResetColor();

            // Pausa para que el usuario vea el mensaje
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();

            return esValido;
        }

        private bool VerificarCredenciales(string nickname, string password)
        {
            var usuario = _usuarioRepository.ObtenerPorNickname(nickname);
            return usuario != null && usuario.Password == password;
        }

        private void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string borde = new string('=', titulo.Length + 4);
            Console.WriteLine(borde);
            Console.WriteLine($"| {titulo} |");
            Console.WriteLine(borde);
            Console.ResetColor();
        }

        private static string LeerPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            return password;
        }
    }
}