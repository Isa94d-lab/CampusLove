using System;
using CampusLove.Domain.Entities;
using CampusLove.Infrastructure.Repositories;

namespace CampusLove.Application.UI
{
    public class MenuRegistro
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PerfilRepository _perfilRepository;
        private readonly ProfesionRepository _profesionRepository;
        private readonly GeneroRepository _generoRepository;
        private readonly EstadoPerfilRepository _estadoPerfilRepository;
        private readonly InteresesRepository _interesesRepository;
        private readonly PerfilInteresesRepository _perfilInteresesRepository;

        public MenuRegistro(
            UsuarioRepository usuarioRepository,
            PerfilRepository perfilRepository,
            ProfesionRepository profesionRepository,
            GeneroRepository generoRepository,
            EstadoPerfilRepository estadoPerfilRepository,
            InteresesRepository interesesRepository,
            PerfilInteresesRepository perfilInteresesRepository)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
            _profesionRepository = profesionRepository;
            _generoRepository = generoRepository;
            _estadoPerfilRepository = estadoPerfilRepository;
            _interesesRepository = interesesRepository;
            _perfilInteresesRepository = perfilInteresesRepository;
        }

        public async Task MostrarMenuAsync()
        {
            Console.Clear();
            MainMenu.MostrarEncabezado("REGISTRO DE USUARIO Y PERFIL");

            // Datos personales
            Console.Write("\n📛 Nombre: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("🧑 Apellido: ");
            string apellido = Console.ReadLine() ?? "";

            Console.Write("🎂 Edad: ");
            int edad = int.TryParse(Console.ReadLine(), out int edadValida) ? edadValida : 0;

            Console.Write("💬 Frase: ");
            string frase = Console.ReadLine() ?? "";

            Console.Write("🎧 Gustos (ej: musica, peliculas, etc): ");
            string gustos = Console.ReadLine() ?? "";

            // Profesión
            var profesiones = (await _profesionRepository.GetAllAsync()).ToList();
            Console.WriteLine("\n💼 Seleccione una profesion:");
            for (int i = 0; i < profesiones.Count; i++)
                Console.WriteLine($"{i + 1}. {profesiones[i].Descripcion}");
            Console.Write("Opción: ");
            int idxProfesion = int.Parse(Console.ReadLine() ?? "1");
            var profesionSeleccionada = profesiones[idxProfesion - 1];

            // Género
            var generos = (await _generoRepository.GetAllAsync()).ToList();
            Console.WriteLine("\n🚻 Seleccione un genero:");
            for (int i = 0; i < generos.Count; i++)
                Console.WriteLine($"{i + 1}. {generos[i].Descripcion}");
            Console.Write("Opción: ");
            int idxGenero = int.Parse(Console.ReadLine() ?? "1");
            var generoSeleccionado = generos[idxGenero - 1];

            // Estado del perfil
            var estados = (await _estadoPerfilRepository.GetAllAsync()).ToList();
            Console.WriteLine("\n📊 Seleccione el estado del perfil:");
            for (int i = 0; i < estados.Count; i++)
                Console.WriteLine($"{i + 1}. {estados[i].Descripcion}");
            Console.Write("Opcion: ");
            int idxEstado = int.Parse(Console.ReadLine() ?? "1");
            var estadoSeleccionado = estados[idxEstado - 1];

            // Crear perfil
            var perfil = new Perfil
            {
                Nombre = nombre,
                Apellido = apellido,
                Edad = edad,
                Frase = frase,
                Gustos = gustos,
                Coins = 10,
                ProfesionId = profesionSeleccionada.Id,
                GeneroId = generoSeleccionado.Id,
                EstadoPerfilId = estadoSeleccionado.Id
            };

            int perfilId = await _perfilRepository.InsertAsync(perfil);

            // Seleccionar intereses
            var intereses = (await _interesesRepository.ObtenerTodosAsync()).ToList();
            Console.WriteLine("\n💘 Seleccione sus intereses romanticos (separados por coma, ej: 1,3):");
            for (int i = 0; i < intereses.Count; i++)
                Console.WriteLine($"{i + 1}. {intereses[i].Tipo}");
            Console.Write("Opción(es): ");
            string? seleccion = Console.ReadLine();
            var seleccionIds = seleccion?.Split(',')
                .Select(s => int.TryParse(s.Trim(), out int id) ? id : -1)
                .Where(id => id > 0 && id <= intereses.Count)
                .ToList() ?? new List<int>();

            foreach (var interesIdx in seleccionIds)
            {
                var interesSeleccionado = intereses[interesIdx - 1];
                var perfilInteres = new PerfilIntereses
                {
                    PerfilId = perfilId,
                    InteresesId = interesSeleccionado.Id
                };
                await _perfilInteresesRepository.AgregarInteresAsync(perfilId, interesSeleccionado.Id);
            }

            // Usuario
            Console.Write("\n👤 Nickname: ");
            string nickname = Console.ReadLine() ?? "";

            Console.Write("🔐 Contraseña: ");
            string password = Console.ReadLine() ?? "";

            var usuario = new Usuario
            {
                PerfilId = perfilId,
                Nickname = nickname,
                Password = password
            };

            await _usuarioRepository.InsertAsync(usuario);

            Console.WriteLine("\n✅ Usuario y perfil registrados exitosamente.");
            Console.WriteLine("Presione una tecla para volver al menu principal...");
            Console.ReadKey();
        }
    }
}
