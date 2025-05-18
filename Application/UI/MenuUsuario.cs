using System;
using CampusLove.Application.UI;
using CampusLove.Domain.Entities;
using CampusLove.Domain.Ports;
using CampusLove.Infrastructure.Repositories;

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
                Console.Clear();
                MostrarEncabezado($"Bienvenido/a {_nickname}");

                Console.WriteLine("1. Mi perfil");
                Console.WriteLine("2. Explorar perfiles ❤️");
                Console.WriteLine("3. Configuracion");
                Console.WriteLine("0. Cerrar sesion");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nSeleccione una opcion: ");
                Console.ResetColor();
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

// Mueve el método fuera de MostrarMenuAsync
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
        MostrarMensaje("No se encontró el perfil asociado al usuario.", ConsoleColor.Red);
        Console.ReadKey();
        return;
    }

    // 3. Obtener los perfiles filtrados por intereses, excluyendo el propio
    var perfiles = await _perfilRepository.GetPerfilesParaBusquedaAsync(perfilActual);

    foreach (var perfil in perfiles)
    {
        if (perfil.Id == perfilActual.Id) continue; // Omitir el propio perfil

        Console.Clear();
        Console.WriteLine("=== PERFIL SUGERIDO ===");
        Console.WriteLine($"Nombre: {perfil.Nombre}");
        Console.WriteLine($"Edad: {perfil.Edad}");
        Console.WriteLine($"Género: {perfil.Genero}");
        Console.WriteLine($"Gustos: {perfil.Gustos}");
        Console.WriteLine($"Descripción: {perfil.Frase}");

        Console.WriteLine("\n¿Te gusta este perfil?");
        Console.WriteLine("1. Sí");
        Console.WriteLine("2. No");
        Console.WriteLine("0. Salir de búsqueda");

        var opcion = Console.ReadLine();
        if (opcion == "1" || opcion == "2")
        {
            bool like = opcion == "1";
            await _interaccionRepository.GuardarLikeAsync(usuarioActual.Id, perfil.UsuarioId, like);
        }
        else if (opcion == "0")
        {
            break;
        }
    }
    Console.WriteLine("Fin de la búsqueda. Presiona cualquier tecla para volver al menú.");
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
            string borde = new string('=', titulo.Length + 4);
            Console.WriteLine(borde);
            Console.WriteLine($"| {titulo} |");
            Console.WriteLine(borde);
            Console.ResetColor();
        }

        private void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }
    }
}
