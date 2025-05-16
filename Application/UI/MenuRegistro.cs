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

        public void MostrarMenu()
        {
            Console.Clear();
            MainMenu.MostrarEncabezado("REGISTRO DE USUARIO Y PERFIL");

            // Registrar Usuario
            Console.Write("\nIngrese el nickname: ");
            string nickname = Console.ReadLine() ?? "";

            Console.Write("Ingrese la contraseña: ");
            string password = Console.ReadLine() ?? "";

            var usuario = new Usuario
            {
                Nickname = nickname,
                Password = password
            };

            // Registrar Perfil
            Console.Write("\nIngrese el nombre: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Ingrese el apellido: ");
            string apellido = Console.ReadLine() ?? "";

            Console.Write("Ingrese la edad: ");
            int edad = int.TryParse(Console.ReadLine(), out int edadValida) ? edadValida : 0;

            Console.Write("Ingrese una frase: ");
            string frase = Console.ReadLine() ?? "";

            Console.Write("Ingrese sus gustos: ");
            string gustos = Console.ReadLine() ?? "";

            Console.Write("Ingrese la cantidad inicial de coins: ");
            int coins = int.TryParse(Console.ReadLine(), out int coinsValidos) ? coinsValidos : 0;

            // Solicitar Profesion
            Console.Write("\nIngrese la profesión: ");
            string profesionNombre = Console.ReadLine() ?? "";
            var profesion = new Profesion { Descripcion = profesionNombre };

            // Solicitar Género
            Console.Write("Ingrese el género: ");
            string generoNombre = Console.ReadLine() ?? "";
            var genero = new Genero { Descripcion = generoNombre };

            // Solicitar Estado de Perfil
            Console.Write("Ingrese el estado del perfil: ");
            string estadoPerfilNombre = Console.ReadLine() ?? "";
            var estadoPerfil = new EstadoPerfil { Descripcion = estadoPerfilNombre };

            var perfil = new Perfil
            {
                Nombre = nombre,
                Apellido = apellido,
                Edad = edad,
                Frase = frase,
                Gustos = gustos,
                Coins = coins,
                Profesion = profesion,
                Genero = genero,
                EstadoPerfil = estadoPerfil
            };

            // Guardar Usuario y Perfil
            _usuarioRepository.Guardar(usuario);
            _perfilRepository.Guardar(perfil);

            Console.WriteLine("\nUsuario y perfil registrados exitosamente.");
            Console.WriteLine("\nPresione cualquier tecla para volver al menú principal...");
            Console.ReadKey();
        }
    }
}