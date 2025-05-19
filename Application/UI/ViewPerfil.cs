using CampusLove.Infrastructure.Repositories;

namespace CampusLove.Application.UI
{
    public class ViewPerfil
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PerfilRepository _perfilRepository;

        public ViewPerfil(UsuarioRepository usuarioRepository, PerfilRepository perfilRepository)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
        }

        public async Task MostrarPerfilAsync(string nickname)
        {
            var usuario = _usuarioRepository.ObtenerPorNicknameAsync(nickname);
            if (usuario == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Usuario no encontrado.");
                Console.ResetColor();
                return;
            }

            var perfil = await _perfilRepository.GetByIdAsync(usuario.PerfilId);
            if (perfil == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Perfil no encontrado.");
                Console.ResetColor();
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            MostrarEncabezado("====== 🌟 MI PERFIL 🌟 ======");
            Console.ResetColor();

            Console.WriteLine($"👤 Nombre: {perfil.Nombre} {perfil.Apellido}");
            Console.WriteLine($"🎂 Edad: {perfil.Edad}");
            Console.WriteLine($"💼 Profesión: {perfil.Profesion?.Descripcion}");
            Console.WriteLine($"🚻 Genero: {perfil.Genero?.Descripcion}");
            Console.WriteLine($"📌 Estado del Perfil: {perfil.EstadoPerfil?.Descripcion}");
            Console.WriteLine($"📝 Frase: {perfil.Frase}");
            Console.WriteLine($"🎯 Gustos: {perfil.Gustos}");
            Console.WriteLine($"🪙 Coins: {perfil.Coins}");

            Console.WriteLine("\nPresione una tecla para volver...");
            Console.ReadKey();
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
    }
}
