using CampusLove.Domain.Entities;

namespace CampusLove.Domain.Ports;

public interface ILikesDiariosRepository
{
    Task<LikesDiarios> ObtenerRegistroDePerfilAsync(int perfilId);
    Task<int> ObtenerLikesDisponiblesAsync(int perfilId);
    Task<bool> RestarLikeAsync(int perfilId);
    Task<bool> EstablecerLikesInicialesAsync(int perfilId, int cantidad);
    Task<bool> ReestablecerLikesSiEsNuevoDiaAsync(int perfilId);
}
