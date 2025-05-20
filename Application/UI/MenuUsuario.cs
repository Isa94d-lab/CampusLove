using CampusLove.Domain.Entities;
using CampusLove.Infrastructure.Repositories;
using MySql.Data.MySqlClient;

namespace CampusLove.Application.UI
{
    public class MenuUsuario
    {
        private readonly string _nickname;
        private readonly ViewPerfil _viewPerfil;
        private readonly PerfilRepository _perfilRepository;
        private readonly InteraccionRepository _interaccionRepository;
        private readonly UsuarioRepository _usuarioRepository;

        public MenuUsuario(string nickname, UsuarioRepository usuarioRepository, PerfilRepository perfilRepository, InteraccionRepository interaccionRepository)
        {
            _nickname = nickname;
            _viewPerfil = new ViewPerfil(usuarioRepository, perfilRepository);
            _perfilRepository = perfilRepository;
            _interaccionRepository = interaccionRepository;
            _usuarioRepository = usuarioRepository;
        }


        public async Task MostrarMenuAsync()
        {
            bool salir = false;

            while (!salir)
            {
                MostrarMenuPrincipal();
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        await _viewPerfil.MostrarPerfilAsync(_nickname);
                        break;
                    case "2":
                        await BuscarParejaAsync();
                        break;
                    case "3":
                        MostrarConfiguracion();
                        break;
                    case "4":
                        await VerMisMatchesAsync();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        MostrarMensaje("Opcion invalida. Intente de nuevo.", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }

        public async Task BuscarParejaAsync()
        {
            // 1. Obtener el usuario actual por nickname
            var usuarioActual = _usuarioRepository.ObtenerPorNicknameAsync(_nickname);
            if (usuarioActual == null)
            {
                MostrarMensaje("No se encontró el usuario actual.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            // 2. Obtener el perfil asociado al usuario actual
            var perfilActual = await _perfilRepository.GetByIdAsync(usuarioActual.PerfilId);
            if (perfilActual == null)
            {
                MostrarMensaje("No se encontro el perfil asociado al usuario.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            // 3. Obtener los perfiles filtrados por intereses, excluyendo el propio
            var perfiles = await _perfilRepository.GetPerfilesParaBusquedaAsync(perfilActual);

            foreach (var perfil in perfiles)
            {
                if (perfil.Id == perfilActual.Id) continue; // Omitir el propio perfil

                MostrarPerfilSugerido(perfil);

                var opcion = Console.ReadLine();
                if (opcion == "1" || opcion == "2")
                {
                    bool like = opcion == "1";
                    await _interaccionRepository.GuardarLikeAsync(usuarioActual.Id, perfil.Id, like);


                    if (like)
                    {
                        var perfilLikeado = await _perfilRepository.GetByIdAsync(perfil.Id);
                        if (perfilLikeado != null)
                        {
                            // Cantidad de coins adicionadas de haber recibido un like
                            perfilLikeado.Coins += 25;
                            await _perfilRepository.ActualizarCoinsAsync(perfilLikeado); // este método lo crearemos
                        }
                    }
                }
                else if (opcion == "0")
                {
                    break;
                }
            }
            Console.WriteLine("Fin de la búsqueda. Presiona cualquier tecla para volver al menú.");
            Console.ReadKey();
            Console.Clear();
        }

        public async Task VerMisMatchesAsync()
        {
            // 1. Obtener el usuario actual y su perfil_id
            var usuarioActual = _usuarioRepository.ObtenerPorNicknameAsync(_nickname);
            if (usuarioActual == null)
            {
                MostrarMensaje("❌ No se encontró el usuario actual.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            int perfilId = usuarioActual.PerfilId;

            // 2. Buscar matches donde el usuario participa
            const string query = @"
                SELECT m.*, p.nombre, p.apellido, p.gustos
                FROM Matchs m
                JOIN Perfil p ON (p.id = m.perfil1_id OR p.id = m.perfil2_id)
                WHERE (m.perfil1_id = @PerfilId OR m.perfil2_id = @PerfilId)
                AND p.id <> @PerfilId
            ";

            using (var command = new MySqlCommand(query, _perfilRepository.Connection))
            {
                command.Parameters.AddWithValue("@PerfilId", perfilId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    Console.Clear();
                    MostrarEncabezado("=== 💖 TUS MATCHES 💖 ===");
                    bool hayMatches = false;

                    while (await reader.ReadAsync())
                    {
                        hayMatches = true;

                        // Mostrar información del match
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"👤 Nombre: {reader["nombre"]} {reader["apellido"]}");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"🎯 Gustos: {reader["gustos"]}");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"📅 Fecha del match: {Convert.ToDateTime(reader["fecha"]).ToShortDateString()}");
                        Console.ResetColor();

                        Console.WriteLine(new string('-', 30));
                    }

                    if (!hayMatches)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Aún no tienes matches.");
                        Console.ResetColor();
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void MostrarConfiguracion()
        {
            MostrarMensaje("Aqui iria la configuracion del usuario.", ConsoleColor.Gray);
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

        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            MostrarEncabezado($"Bienvenido/a {_nickname} 🌟");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🌟 1. Mi perfil");
            Console.WriteLine("❤️ 2. Explorar perfiles");
            Console.WriteLine("⚙️ 3. Configuración");
            Console.WriteLine("💌 4. Ver mis matches");
            Console.WriteLine("🚪 0. Cerrar sesión");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nSeleccione una opción: ");
            Console.ResetColor();
        }

        private void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n💬 {mensaje}");
            Console.ResetColor();
        }

        private void MostrarPerfilSugerido(Perfil perfil)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            MostrarEncabezado("=== PERFIL SUGERIDO ===");
            Console.ResetColor();

            Console.WriteLine($"👤 Nombre: {perfil.Nombre}");
            Console.WriteLine($"👤 Apellido: {perfil.Apellido}");
            Console.WriteLine($"🎂 Edad: {perfil.Edad}");
            Console.WriteLine($"🎨 Gustos: {perfil.Gustos}");
            Console.WriteLine($"📝 Descripción: {perfil.Frase}");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n💖 ¿Te gusta este perfil? ->");
            Console.ResetColor();
            Console.WriteLine("1. ❤️ Sí");
            Console.WriteLine("2. 💔 No");
            Console.WriteLine("0. 🚪 Salir de búsqueda");
        }
    }
}
