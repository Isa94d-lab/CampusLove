using CampusLove.Infrastructure.Repositories;

namespace CampusLove.Application.UI
{
    public class ViewEstadisticas
    {
        private readonly EstadisticasRepository _repo;

        public ViewEstadisticas(EstadisticasRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> MostrarEstadisticasAsync()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                MainMenu.MostrarEncabezado("📊 ESTADÍSTICAS DEL SISTEMA");

                Console.WriteLine("1. Perfil con más likes recibidos");
                Console.WriteLine("2. Perfil con más matches realizados");
                Console.WriteLine("3. Usuario con más interacciones");
                Console.WriteLine("4. Promedio de edad de los perfiles");
                Console.WriteLine("0. Volver");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nSeleccione una opción: ");
                Console.ResetColor();

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        await MostrarPerfilMasLikeadoAsync();
                        break;
                    case "2":
                        await MostrarPerfilMasMatchAsync();
                        break;
                    case "3":
                        await MostrarUsuarioMasInteraccionesAsync();
                        break;
                    case "4":
                        await MostrarPromedioEdadAsync();
                        break;
                    case "0":
                        volver = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción inválida. Presione una tecla para continuar...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }

            return true;
        }

        private async Task MostrarPerfilMasLikeadoAsync()
        {
            var resultado = await _repo.ObtenerPerfilConMasLikesAsync();
            Console.Clear();
            MainMenu.MostrarEncabezado("❤️ Perfil con más likes recibidos");

            if (resultado != null)
            {
                Console.WriteLine($"Perfil: {resultado.Value.Nombre} {resultado.Value.Apellido}");
                Console.WriteLine($"Likes recibidos: {resultado.Value.TotalLikes}");
            }
            else
            {
                Console.WriteLine("No hay datos disponibles.");
            }

            Console.WriteLine("\nPresione una tecla para volver...");
            Console.ReadKey();
        }

        private async Task MostrarPerfilMasMatchAsync()
        {
            var resultado = await _repo.ObtenerPerfilConMasMatchesAsync();
            Console.Clear();
            MainMenu.MostrarEncabezado("💘 Perfil con más matches realizados");

            if (resultado != null)
            {
                Console.WriteLine($"Perfil: {resultado.Value.Nombre} {resultado.Value.Apellido}");
                Console.WriteLine($"Matches realizados: {resultado.Value.TotalMatches}");
            }
            else
            {
                Console.WriteLine("No hay datos disponibles.");
            }

            Console.WriteLine("\nPresione una tecla para volver...");
            Console.ReadKey();
        }

        private async Task MostrarUsuarioMasInteraccionesAsync()
        {
            var resultado = await _repo.ObtenerUsuarioConMasInteraccionesAsync();
            Console.Clear();
            MainMenu.MostrarEncabezado("🎯 Usuario con más interacciones");

            if (resultado != null)
            {
                Console.WriteLine($"Usuario: {resultado.Value.Nickname}");
                Console.WriteLine($"Interacciones realizadas: {resultado.Value.TotalInteracciones}");
            }
            else
            {
                Console.WriteLine("No hay datos disponibles.");
            }

            Console.WriteLine("\nPresione una tecla para volver...");
            Console.ReadKey();
        }

        private async Task MostrarPromedioEdadAsync()
        {
            double promedio = await _repo.ObtenerPromedioEdadPerfilesAsync();
            Console.Clear();
            MainMenu.MostrarEncabezado("📈 Promedio de edad de perfiles");

            Console.WriteLine($"Promedio de edad: {promedio:F2} años");

            Console.WriteLine("\nPresione una tecla para volver...");
            Console.ReadKey();
        }
    }
}
