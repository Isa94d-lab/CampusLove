namespace CampusLove.Domain.Ports;

public interface IInteraccionRepository
{
    Task<bool> DarLikeAsync(int perfilOrigenId, int perfilDestinoId);
    Task<bool> DarDislikeAsync(int perfilOrigenId, int perfilDestinoId);
    Task<bool> YaInteraccionExisteAsync(int perfilOrigenId, int perfilDestinoId);
}
