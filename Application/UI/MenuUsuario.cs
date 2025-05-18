using System;
using CampusLove.Application.UI;
using CampusLove.Domain.Entities;
using CampusLove.Infrastructure.Repositories;

namespace CampusLove.Application.UI
{
    public class MenuUsuario
    {
        private readonly string _nickname;
        private readonly ViewPerfil _viewPerfil;
        public MenuUsuario(string nickname, UsuarioRepository usuarioRepository, PerfilRepository perfilRepository)
        {
            _nickname = nickname;
            _viewPerfil = new ViewPerfil(usuarioRepository, perfilRepository);
        }


        public async Task MostrarMenuAsync()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarEncabezado($"Bienvenido/a {_nickname}");

                Console.WriteLine("1. Ver mi perfil");
                Console.WriteLine("2. Buscar Match ❤️");
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
                        BuscarPareja();
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

            MostrarMensaje("\n¡Sesion cerrada con exito!", ConsoleColor.DarkCyan);
        }

        private void BuscarPareja()
        {
            MostrarMensaje("Aqui iria la logica para buscar pareja. 🥰", ConsoleColor.Magenta);
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
