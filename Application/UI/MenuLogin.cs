using CampusLove.Infrastructure.Repositories;

namespace CampusLove.Application.UI
{
    public class MenuLogin
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PerfilRepository _perfilRepository;
        private readonly InteraccionRepository _interaccionRepository;

        public MenuLogin(UsuarioRepository usuarioRepository, PerfilRepository perfilRepository, InteraccionRepository interaccionRepository)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
            _interaccionRepository = interaccionRepository;
        }

        public async Task<bool> MostrarLoginAsync()
        {
            Console.Clear();
            MostrarEncabezado("🔑 LOGIN 🔑");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n👤 Nickname: ");
            Console.ResetColor();
            string nickname = Console.ReadLine() ?? string.Empty;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("🔒 Password: ");
            Console.ResetColor();
            string password = LeerPassword();

            bool esValido = VerificarCredenciales(nickname, password);

            if (esValido)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ ¡Login exitoso! Bienvenido/a a Campus Love 💖");
                Console.ResetColor();

                // Conexión con el menú del usuario si el login es exitoso
                var menuUsuario = new MenuUsuario(nickname, _usuarioRepository, _perfilRepository, _interaccionRepository);
                await menuUsuario.MostrarMenuAsync();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Nickname o contraseña incorrectos. Intente de nuevo.");
                Console.ResetColor();
            }

            // Pausa para que el usuario vea el mensaje
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();

            return esValido;
        }

        private bool VerificarCredenciales(string nickname, string password)
        {
            var usuario = _usuarioRepository.ObtenerPorNicknameAsync(nickname);
            return usuario != null && usuario.Password == password;
        }

        private void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string borde = new string('═', titulo.Length + 6);
            Console.WriteLine($"╔{borde}╗");
            Console.WriteLine($"║  {titulo}    ║");
            Console.WriteLine($"╚{borde}╝");
            Console.ResetColor();
        }

        private static string LeerPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            Console.ForegroundColor = ConsoleColor.Cyan;
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
            Console.ResetColor();
            return password;
        }
    }
}