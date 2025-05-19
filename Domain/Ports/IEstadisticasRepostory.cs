namespace CampusLove.Domain.Ports
{
    public interface IEstadisticasRepository
    {
        Task<(string Nombre, string Apellido, int TotalLikes)?> ObtenerPerfilConMasLikesAsync();
        Task<(string Nombre, string Apellido, int TotalMatches)?> ObtenerPerfilConMasMatchesAsync();
        Task<(string Nickname, int TotalInteracciones)?> ObtenerUsuarioConMasInteraccionesAsync();
        Task<double> ObtenerPromedioEdadPerfilesAsync();
    }
}