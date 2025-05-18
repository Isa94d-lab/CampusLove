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

        public MenuRegistro(
            UsuarioRepository usuarioRepository,
            PerfilRepository perfilRepository,
            ProfesionRepository profesionRepository,
            GeneroRepository generoRepository,
            EstadoPerfilRepository estadoPerfilRepository)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
            _profesionRepository = profesionRepository;
            _generoRepository = generoRepository;
            _estadoPerfilRepository = estadoPerfilRepository;
        }

        public async Task MostrarMenuAsync()
        {
            Console.Clear();
            MainMenu.MostrarEncabezado("REGISTRO DE USUARIO Y PERFIL");

            // Datos del perfil
            Console.Write("\n📛 Nombre: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("🧑 Apellido: ");
            string apellido = Console.ReadLine() ?? "";

            Console.Write("🎂 Edad: ");
            int edad = int.TryParse(Console.ReadLine(), out int edadValida) ? edadValida : 0;

            Console.Write("💬 Frase: ");
            string frase = Console.ReadLine() ?? "";

            Console.Write("❤️ Gustos: ");
            string gustos = Console.ReadLine() ?? "";

            // Selección de profesión
            var profesiones = (await _profesionRepository.GetAllAsync()).ToList();
            Console.WriteLine("\n💼 Seleccione una profesión:");
            for (int i = 0; i < profesiones.Count; i++)
                Console.WriteLine($"{i + 1}. {profesiones[i].Descripcion}");
            Console.Write("Opción: ");
            int idxProfesion = int.Parse(Console.ReadLine() ?? "1");
            var profesionSeleccionada = profesiones[idxProfesion - 1];

            // Selección de género
            var generos = (await _generoRepository.GetAllAsync()).ToList();
            Console.WriteLine("\n🚻 Seleccione un género:");
            for (int i = 0; i < generos.Count; i++)
                Console.WriteLine($"{i + 1}. {generos[i].Descripcion}");
            Console.Write("Opción: ");
            int idxGenero = int.Parse(Console.ReadLine() ?? "1");
            var generoSeleccionado = generos[idxGenero - 1];

            // Selección de estado de perfil
            var estados = (await _estadoPerfilRepository.GetAllAsync()).ToList();
            Console.WriteLine("\n📊 Seleccione el estado del perfil:");
            for (int i = 0; i < estados.Count; i++)
                Console.WriteLine($"{i + 1}. {estados[i].Descripcion}");
            Console.Write("Opción: ");
            int idxEstado = int.Parse(Console.ReadLine() ?? "1");
            var estadoSeleccionado = estados[idxEstado - 1];

            // Crear y guardar perfil primero
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

            // Este método InsertAsync debe devolver el id generado para el perfil
            int perfilId = await _perfilRepository.InsertAsync(perfil);

            // Ahora pedir datos para usuario
            Console.Write("\n Nickname: ");
            string nickname = Console.ReadLine() ?? "";

            Console.Write(" Contraseña: ");
            string password = Console.ReadLine() ?? "";

            // Crear y guardar usuario asignándole el perfilId recién creado
            var usuario = new Usuario
            {
                PerfilId = perfilId,
                Nickname = nickname,
                Password = password
            };

            await _usuarioRepository.InsertAsync(usuario);

            Console.WriteLine("\n✅ Usuario y perfil registrados exitosamente.");
            Console.WriteLine("Presione una tecla para volver al menú principal...");
            Console.ReadKey();
        }
    }

}